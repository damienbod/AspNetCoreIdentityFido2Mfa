using AspNetCoreIdentityFido2Mfa.Data;
using Fido2NetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityFido2Mfa
{
    public class Fido2Storage
    {
       private readonly ApplicationDbContext _applicationDbContext;

        public Fido2Storage(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
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

            return Task.FromResult(
                _applicationDbContext.Users
                    .Where(u => Encoding.UTF8.GetBytes(u.UserName)
                    .SequenceEqual(cred.UserId))
                    .Select(u => new Fido2User
                    {
                        DisplayName = u.UserName,
                        Name = u.UserName,
                        Id = Encoding.UTF8.GetBytes(u.UserName) // byte representation of userID is required
                    }
            ).ToList());
        }
    }
}
