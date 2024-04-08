function newId() {
    let bytes = new Uint8Array(15);
    crypto.getRandomValues(bytes);
    return asBase62(bytes);
}

const SIXTY_ONE = 61;
const SIXTY_TWO = 62;

function asBase62(bytes) {
    let stringId = ""
    let sum = 0;
    for (let i = 0; i < bytes.length; i += 1) {
        let max61 = bytes[i] & SIXTY_ONE;
        stringId += SIXTY_TWO_CHARS[max61];
        sum += max61
    }
    return stringId + SIXTY_TWO_CHARS[sum % SIXTY_TWO];
}

function isValid(stringId) {
    if (stringId.length !== 16) return false;
    let sum = 0;
    for (let i = 0; i < stringId.length - 1; i += 1) {
        if (SIXTY_TWO_CHARS.indexOf(stringId[i]) === -1) return false;
        sum += SIXTY_TWO_CHARS.indexOf(stringId[i]);
    }
    return (sum % SIXTY_TWO) === SIXTY_TWO_CHARS.indexOf(stringId[stringId.length - 1]);
}

const SIXTY_TWO_CHARS = [
    '0', '1', '2', '3', '4', '5', '6', '7',
    '8', '9', 'A', 'B', 'C', 'D', 'E', 'F',
    'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
    'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
    'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
    'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
    'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
    'u', 'v', 'w', 'x', 'y', 'z'
];

exports.newId = newId;
exports.isValid = isValid;
