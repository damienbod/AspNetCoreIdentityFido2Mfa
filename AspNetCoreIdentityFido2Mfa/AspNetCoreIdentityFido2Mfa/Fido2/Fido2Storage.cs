using AspNetCoreIdentityFido2Mfa.Data;
using Fido2NetLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityFido2Mfa
{
    public class Fido2Storage
    {
        ConcurrentDictionary<string, Fido2User> storedUsers = new ConcurrentDictionary<string, Fido2User>();
        private readonly ApplicationDbContext _applicationDbContext;

        public Fido2Storage(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Fido2User GetOrAddUser(string username, Func<Fido2User> addCallback)
        {
            return storedUsers.GetOrAdd(username, addCallback());
        }

        public Fido2User GetUser(string username)
        {
            storedUsers.TryGetValue(username, out var user);
            return user;
        }

        public List<FidoStoredCredential> GetCredentialsByUser(Fido2User user)
        {
            return _applicationDbContext.FidoStoredCredential.Where(c => c.UserId.SequenceEqual(user.Id)).ToList();
        }

        public FidoStoredCredential GetCredentialById(byte[] id)
        {
            return _applicationDbContext.FidoStoredCredential.Where(c => c.Descriptor.Id.SequenceEqual(id)).FirstOrDefault();
        }

        public Task<List<FidoStoredCredential>> GetCredentialsByUserHandleAsync(byte[] userHandle)
        {
            return Task.FromResult(_applicationDbContext.FidoStoredCredential.Where(c => c.UserHandle.SequenceEqual(userHandle)).ToList());
        }

        public void UpdateCounter(byte[] credentialId, uint counter)
        {
            var cred = _applicationDbContext.FidoStoredCredential.Where(c => c.Descriptor.Id.SequenceEqual(credentialId)).FirstOrDefault();
            cred.SignatureCounter = counter;
        }

        public void AddCredentialToUser(Fido2User user, FidoStoredCredential credential)
        {
            credential.UserId = user.Id;
            _applicationDbContext.FidoStoredCredential.Add(credential);
        }

        public Task<List<Fido2User>> GetUsersByCredentialIdAsync(byte[] credentialId)
        {
            // our in-mem storage does not allow storing multiple users for a given credentialId. Yours shouldn't either.
            var cred = _applicationDbContext.FidoStoredCredential.Where(c => c.Descriptor.Id.SequenceEqual(credentialId)).FirstOrDefault();

            if (cred == null) return Task.FromResult(new List<Fido2User>());

            return Task.FromResult(storedUsers.Where(u => u.Value.Id.SequenceEqual(cred.UserId)).Select(u => u.Value).ToList());
        }
    }
}
