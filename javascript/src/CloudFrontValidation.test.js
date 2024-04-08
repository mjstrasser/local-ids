const CfValidation = require('./CloudFrontValidation');

describe('handler()', () => {
    test('returns the request if a valid LocalId string is the last segment of the URI', () => {
        let event = {request: {uri: '/l/0123456789ABCDEh'}};
        let result = CfValidation.handler(event);
        expect(result).toEqual(event.request)
    });
    test('returns a 404 Not Found response if there is not a valid LocalId string as the last segment of the URI', () => {
        let event = {request: {uri: '/l/0123456789ABCDEFG'}};
        let result = CfValidation.handler(event);
        expect(result.statusCode).toEqual(404);
    });
});
