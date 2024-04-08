const LocalId = require('./LocalId');

const RESPONSE_404 = {
    statusCode: 404,
    statusDescription: 'Not Found'
};

// Matches a URI that ends with a LocalId
const LOCAL_ID_LAST = /[^\/]+$/;

/*
 * Simple AWS CloudFront function that validates a LocalId string as the
 * last part of a URI path.
 *
 * If the LocalId string is valid it returns the request, otherwise it
 * returns 404 Not Found response.
 */
function handler(event) {
    const request = event.request;
    const match = request.uri.match(LOCAL_ID_LAST);
    if (match) {
        if (LocalId.isValid(match[0])) return request;
    }
    return RESPONSE_404;
}

exports.handler = handler;
