document.getElementById('signin').addEventListener('submit', handleSignInSubmit);

async function handleSignInSubmit(event) {
    event.preventDefault();

    let username = this.username.value;

    // possible values: preferred, required, discouraged
    let user_verification = "required";

    // prepare form post data
    var formData = new FormData();
    formData.append('username', username);
    formData.append('userVerification', user_verification);

    // send to server for registering
    let makeAssertionOptions;
    try {
        var res = await fetch(getFolder() + '/pwassertionOptions', {
            method: 'POST', // or 'PUT'
            body: formData, // data can be `string` or {object}!
            headers: {
                'Accept': 'application/json',
                'RequestVerificationToken': document.getElementById('RequestVerificationToken').value
            }
        });

        makeAssertionOptions = await res.json();
    } catch (e) {
        showErrorAlert("Request to server failed", e);
    }

    //console.log("Assertion Options Object", makeAssertionOptions);

    // show options error to user
    if (makeAssertionOptions.status !== "ok") {
        console.log("Error creating assertion options");
        console.log(makeAssertionOptions.errorMessage);
        showErrorAlert(makeAssertionOptions.errorMessage);
        return;
    }

    // todo: switch this to coercebase64
    const challenge = makeAssertionOptions.challenge.replace(/-/g, "+").replace(/_/g, "/");
    makeAssertionOptions.challenge = Uint8Array.from(atob(challenge), c => c.charCodeAt(0));

    // fix escaping. Change this to coerce
    makeAssertionOptions.allowCredentials.forEach(function (listItem) {
        var fixedId = listItem.id.replace(/\_/g, "/").replace(/\-/g, "+");
        listItem.id = Uint8Array.from(atob(fixedId), c => c.charCodeAt(0));
    });

    //console.log("Assertion options", makeAssertionOptions);

    Swal.fire({
        title: 'Logging In...',
        text: 'Tap your security key to login.',
        imageUrl: getFolder() + "/images/securitykey.min.svg",
        showCancelButton: true,
        showConfirmButton: false,
        focusConfirm: false,
        focusCancel: false
    });

    // ask browser for credentials (browser will ask connected authenticators)
    let credential;
    try {
        credential = await navigator.credentials.get({ publicKey: makeAssertionOptions })
    } catch (err) {
        showErrorAlert(err.message ? err.message : err);
    }

    try {
        await verifyAssertionWithServer(credential);
    } catch (e) {
        showErrorAlert("Could not verify assertion", e);
    }
}

async function verifyAssertionWithServer(assertedCredential) {

    // Move data into Arrays incase it is super long
    let authData = new Uint8Array(assertedCredential.response.authenticatorData);
    let clientDataJSON = new Uint8Array(assertedCredential.response.clientDataJSON);
    let rawId = new Uint8Array(assertedCredential.rawId);
    let sig = new Uint8Array(assertedCredential.response.signature);
    let userHandle = new Uint8Array(assertedCredential.response.userHandle);
    const data = {
        id: assertedCredential.id,
        rawId: coerceToBase64Url(rawId),
        type: assertedCredential.type,
        extensions: assertedCredential.getClientExtensionResults(),
        response: {
            authenticatorData: coerceToBase64Url(authData),
            clientDataJson: coerceToBase64Url(clientDataJSON),
            userHandle: userHandle !== null && userHandle.length > 0 ? coerceToBase64Url(userHandle) : null,
            signature: coerceToBase64Url(sig)
        }
    };

    let response;
    try {
        let res = await fetch(getFolder() + "/pwmakeAssertion", {
            method: 'POST', // or 'PUT'
            body: JSON.stringify(data), // data can be `string` or {object}!
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.getElementById('RequestVerificationToken').value
            }
        });

        response = await res.json();
    } catch (e) {
        showErrorAlert("Request to server failed", e);
        throw e;
    }

    //console.log("Assertion Object", response);

    // show error
    if (response.status !== "ok") {
        console.log("Error doing assertion");
        console.log(response.errorMessage);
        showErrorAlert(response.errorMessage);
        return;
    }

    // show success message
    await Swal.fire({
        title: 'Logged In!',
        text: 'You\'re logged in successfully.',
        // type: 'success',
        timer: 2000
    });

    let returnUrl = findGetParameter('ReturnUrl');
    if (!returnUrl) {
        returnUrl = getFolder();
    }
    window.location.href = returnUrl;
}
