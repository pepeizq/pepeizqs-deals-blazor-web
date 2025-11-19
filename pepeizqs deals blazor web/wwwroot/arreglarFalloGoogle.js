function arreglarFalloGoogle() {
    try {
        if (!navigator.storage) {
            navigator.storage = {};
        }

        if (typeof navigator.storage.persist !== 'function') {
            navigator.storage.persist = function () {
                return Promise.resolve(false);
            };
        }

        if (!navigator.persistentStorage) {
            navigator.persistentStorage = {
                persist: navigator.storage.persist
            };
        }
    } catch (e) {
        console.warn("Error aplicando el polyfill de persistent storage:", e);
    }
}