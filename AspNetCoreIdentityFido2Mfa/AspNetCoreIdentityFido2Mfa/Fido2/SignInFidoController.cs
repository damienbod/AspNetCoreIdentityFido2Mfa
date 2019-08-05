using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fido2NetLib.Objects;
using Fido2NetLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Fido2NetLib.Development;
using static Fido2NetLib.Fido2;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCoreIdentityFido2Mfa
{

    [Route("api/[controller]")]
    public class SignInFidoController : Controller
    {
        private Fido2 _lib;
        public static IMetadataService _mds;
        private string _origin;
        private readonly Fido2Storage _fido2Storage;

        public SignInFidoController(IConfiguration config, Fido2Storage fido2Storage)
        {
            _fido2Storage = fido2Storage;
            var MDSAccessKey = config["fido2:MDSAccessKey"];
            var MDSCacheDirPath = config["fido2:MDSCacheDirPath"] ?? Path.Combine(Path.GetTempPath(), "fido2mdscache"); 
            _mds = string.IsNullOrEmpty(MDSAccessKey) ? null : MDSMetadata.Instance(MDSAccessKey, MDSCacheDirPath);
            if (null != _mds)
            {
                if (false == _mds.IsInitialized())
                    _mds.Initialize().Wait();
            }
            _origin = config["fido2:origin"];
            _lib = new Fido2(new Fido2Configuration()
            {
                ServerDomain = config["fido2:serverDomain"],
                ServerName = "Fido2 test",
                Origin = _origin,
                // Only create and use Metadataservice if we have an acesskey
                MetadataService = _mds,
                TimestampDriftTolerance = config.GetValue<int>("fido2:TimestampDriftTolerance")
            });
        }

        private string FormatException(Exception e)
        {
            return string.Format("{0}{1}", e.Message, e.InnerException != null ? " (" + e.InnerException.Message + ")" : "");
        }

        [HttpPost]
        [Route("/assertionOptions")]
        public ActionResult AssertionOptionsPost([FromForm] string username, [FromForm] string userVerification)
        {
            try
            {
                var existingCredentials = new List<PublicKeyCredentialDescriptor>();

                if (!string.IsNullOrEmpty(username))
                {
                    // 1. Get user from DB
                    var user = _fido2Storage.GetUser(username);
                    if (user == null) throw new ArgumentException("Username was not registered");

                    // 2. Get registered credentials from database
                    existingCredentials = _fido2Storage.GetCredentialsByUser(user).Select(c => c.Descriptor).ToList();
                }

                var exts = new AuthenticationExtensionsClientInputs() { SimpleTransactionAuthorization = "FIDO", GenericTransactionAuthorization = new TxAuthGenericArg { ContentType = "text/plain", Content = new byte[] { 0x46, 0x49, 0x44, 0x4F } }, UserVerificationIndex = true, Location = true, UserVerificationMethod = true };

                // 3. Create options
                var uv = string.IsNullOrEmpty(userVerification) ? UserVerificationRequirement.Discouraged : userVerification.ToEnum<UserVerificationRequirement>();
                var options = _lib.GetAssertionOptions(
                    existingCredentials,
                    uv,
                    exts
                );

                // 4. Temporarily store options, session/in-memory cache/redis/db
                HttpContext.Session.SetString("fido2.assertionOptions", options.ToJson());

                // 5. Return options to client
                return Json(options);
            }

            catch (Exception e)
            {
                return Json(new AssertionOptions { Status = "error", ErrorMessage = FormatException(e) });
            }
        }

        [HttpPost]
        [Route("/makeAssertion")]
        public async Task<JsonResult> MakeAssertion([FromBody] AuthenticatorAssertionRawResponse clientResponse)
        {
            try
            {
                // 1. Get the assertion options we sent the client
                var jsonOptions = HttpContext.Session.GetString("fido2.assertionOptions");
                var options = AssertionOptions.FromJson(jsonOptions);

                // 2. Get registered credential from database
                var creds = _fido2Storage.GetCredentialById(clientResponse.Id);

                if(creds == null)
                {
                    throw new Exception("Unknown credentials");
                }

                // 3. Get credential counter from database
                var storedCounter = creds.SignatureCounter;

                // 4. Create callback to check if userhandle owns the credentialId
                IsUserHandleOwnerOfCredentialIdAsync callback = async (args) =>
                {
                    var storedCreds = await _fido2Storage.GetCredentialsByUserHandleAsync(args.UserHandle);
                    return storedCreds.Exists(c => c.Descriptor.Id.SequenceEqual(args.CredentialId));
                };

                // 5. Make the assertion
                var res = await _lib.MakeAssertionAsync(clientResponse, options, creds.PublicKey, storedCounter, callback);

                // 6. Store the updated counter
                _fido2Storage.UpdateCounter(res.CredentialId, res.Counter);

                // 7. return OK to client
                return Json(res);
            }
            catch (Exception e)
            {
                return Json(new AssertionVerificationResult { Status = "error", ErrorMessage = FormatException(e) });
            }
        }
    }
}
