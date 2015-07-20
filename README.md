NonceUtil-C#
=============

A tiny C# NONCE generator with variable time-outs and individual salts, no database required. Inspired by https://github.com/timostamm/NonceUtil-PHP


## Usage

Generate a NONCE with one minute lifetime:

    var nonce = NonceUtil.GenerateNonce("myNonceSecret", 60);

Generate a NONCE with the default lifetime (180 seconds / 3 minutes):

    var nonce = NonceUtil.GenerateNonce("myNonceSecret", 60);


Validate the NONCE:

    var result = NonceUtil.CheckNonce("myNonceSecret", nonce);


You have to define a secret on the server side and pass it to the `GenerateNonce()` and `CheckNonce()` methods as the first argument. The secret string must be at least 10 characters long. 

The seconds argument to `GenerateNonce()` defines the lifetime of the NONCE in seconds. There are no upper restrictions on the lifetime, however if specified, the lifetime must be greater than zero. 



## Background

The generated NONCE is technically a NONCE-word, because it consists of a long string, rather than a number. Let's have a look at a generated NONCE:

    RR0vw04FjgwQISK,1437400734.45616,ZhKyv5CzfNOSrYGYS3gWE/8hssQrA8uup9v8eSckUqQ=

Each NONCE consists of three parts, separated by a `,`. The first part is a salt, which is randomly created for each NONCE. The second part is the UNIX time at which the NONCE expires. The third part is a SHA1 hash of the salt, secret, and the time of expiration combined. 

In order to validate the NONCE, we recalculate the hash and compare it to the hash in the NONCE. If the hash is valid, we are sure that the NONCE originates from us and was not tampered with, and we can simply check if NONCE is expired.

## Examples


See NonceUtil.Tests
