# `LocalId`: a local ID in JavaScript

## Create a `LocalId`

To create a `LocalId`:

```javascript
    // Random ID.
    let randomId = LocalId.newId()
```

## Check validity

The static method `LocalId.isValid()` returns `false` if the string is
the wrong length, contains invalid characters, or if the last character is not a correct
check on the preceding 15.

```javascript
    LocalId.isValid("n9Gmv0mfDWuy08DG") // true
    LocalId.isValid("n9Gmv0mfDWuy08DH") // false
```

## Example usage: AWS CloudFront function

An example
[AWS CloudFront function](https://docs.aws.amazon.com/AmazonCloudFront/latest/DeveloperGuide/edge-functions.html)
that validates `LocalId` string values at the edge is:

```javascript
function handler(event) {
  const request = event.request;
  const match = request.uri.match(LOCAL_ID_LAST);
  if (match) {
    if (LocalId.isValid(match[0])) return request;
  }
  return RESPONSE_404;
}
```
The complete example is [here](./cloudfront-validation.js).
