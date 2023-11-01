# ASP.NET Core Identity with FIDO2 WebAuthn, MFA and Passwordless 

[![.NET](https://github.com/damienbod/AspNetCoreIdentityFido2Mfa/workflows/.NET/badge.svg)](https://github.com/damienbod/AspNetCoreIdentityFido2Mfa/actions?query=workflow%3A.NET)

## Database

```
Add-Migration "init_identity" 
```

```
Update-Database
```

## Blogs

[ASP.NET Core Identity with FIDO2 WebAuthn MFA](https://damienbod.com/2019/08/06/asp-net-core-identity-with-fido2-webauthn-mfa/)

[Adding FIDO2 Passwordless authentication to an ASP.NET Core Identity App](https://damienbod.com/2019/10/18/adding-fido2-passwordless-authentication-to-an-asp-net-core-identity-app/)

## History

- 2023-08-18 Updated packages, revert to Fido2 3.0.1 => problems with beta version
- 2023-06-20 Updated packages, Fido2 4.0.0-beta1
- 2023-04-28 Updated packages
- 2023-02-18 Updated packages, improved passwordless login
- 2022-12-31 Updated to .NET 7, fix passwordless login
- 2022-10-15 Updated nuget packages
- 2022-07-31 Updated Fido2 nuget package to 3.0.0 and npm, nuget packages
- 2022-06-29 Updated Fido2 nuget package to 3.0.0-beta6
- 2022-06-12 Migrate to latest identity, bootstrap 5, updated packages, nullable, implicit usings
- 2022-02-13 Updated packages
- 2021-12-16 Updated to .NET 6
- 2021-08-20 Update npm packages
- 2021-04-03 Update npm packages
- 2021-03-20 Update npm, nuget packages
- 2021-01-10 Update .NET 5, code clean up
- 2020-09-11 Added Anti-forgery protection, Updated Nuget packages, npm packages
- 2020-08-28 Fix Bad URL register FIDO key, Updated Nuget packages, npm packages
- 2020-05-06 Fix FIDO2 database model
- 2020-05-05 Updated nuget packages, FIDO2 1.1.0
- 2020-02-28 Updated nuget packages, add support for multiple keys per user
- 2019-12-29 Update to .NET Core 3.1
- 2019-10-18 Added example for FIDO2 passwordless
- 2019-10-07 Updated to .NET Core 3.0
- 2019-09-20 Updated to .NET Core 3.0 rc1
- 2019-09-06 Updated to .NET Core 3.0 preview 9
- 2019-08-13 Updated to .NET Core 3.0 preview 8

## Links

- https://github.com/abergs/fido2-net-lib
- https://webauthn.io/
- https://webauthn.guide
- https://www.youtube.com/watch?v=qgZ3JO2khFg
- https://www.yubico.com/products/yubikey-hardware/
- https://www.youtube.com/watch?v=2KfZJRsacNM
- https://www.troyhunt.com/beyond-passwords-2fa-u2f-and-google-advanced-protection/
- https://fidoalliance.org/fido2/
- https://www.w3.org/TR/webauthn/
- https://www.scottbrady91.com/FIDO/A-FIDO2-Primer-and-Proof-of-Concept-using-ASPNET-Core
- https://github.com/herrjemand/awesome-webauthn
- https://developers.yubico.com/FIDO2/Libraries/Using_a_library.html
- https://medium.com/@herrjemand
- https://github.com/w3c/webauthn
- https://passthesalt.ubicast.tv/videos/2020-replacing-passwords-with-fido2/
- https://www.youtube.com/watch?v=S8Z1p_2yAzg&feature=youtu.be
