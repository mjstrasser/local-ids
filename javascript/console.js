const LocalId = require('./local-id');

for (i = 0; i < 1000; i++) {
    console.log(LocalId.newId());
}
