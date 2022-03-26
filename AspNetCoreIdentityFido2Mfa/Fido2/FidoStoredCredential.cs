using Fido2NetLib.Objects;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2Identity;

/// <summary>
/// Represents a WebAuthn credential.
/// </summary>
public class FidoStoredCredential
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user name for this user.
    /// </summary>
    public string Username { get; set; }

    public byte[] UserId { get; set; }

    /// <summary>
    /// Gets or sets the public key for this user.
    /// </summary>
    public byte[] PublicKey { get; set; }

    /// <summary>
    /// Gets or sets the user handle for this user.
    /// </summary>
    public byte[] UserHandle { get; set; }

    public uint SignatureCounter { get; set; }

    public string CredType { get; set; }
    
    /// <summary>
    /// Gets or sets the registration date for this user.
    /// </summary>
    public DateTime RegDate { get; set; }

    /// <summary>
    /// Gets or sets the Authenticator Attestation GUID (AAGUID) for this user.
    /// </summary>
    /// <remarks>
    /// An AAGUID is a 128-bit identifier indicating the type of the authenticator.
    /// </remarks>
    public Guid AaGuid { get; set; }

    [NotMapped]
    public PublicKeyCredentialDescriptor Descriptor
    {
        get { return string.IsNullOrWhiteSpace(DescriptorJson) ? null : JsonConvert.DeserializeObject<PublicKeyCredentialDescriptor>(DescriptorJson); }
        set { DescriptorJson = JsonConvert.SerializeObject(value); }
    }
    public string DescriptorJson { get; set; }
}
