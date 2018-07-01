export default {
    AllFlashcards: () => {
        return fetch('/api/Flashcard', {
            method: 'get'
        }).then(function (response) {
            return response.json();
        }).then(function(response) {
            return response;
        });
    },
    GetFlashcard: (id) => {
        return fetch('/api/Flashcard/' + id, {
            method: 'get'
        }).then(function (response) {
            return response.json();
        }).then(function(response) {
            return response;
        });
    },
}