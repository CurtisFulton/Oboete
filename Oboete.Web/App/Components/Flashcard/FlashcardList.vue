<template>
    <div id="app">
        <h1>Flashcards</h1>
        <p>Status {{ info }}</p>
        <ul>
            <li v-for="flashcard in flashcards" :key="flashcard.flashcardID">
                <router-link :to="'/Flashcard/' + flashcard.flashcardID" exact>Go to flashcard {{flashcard.flashcardID}}</router-link>
            </li>
        </ul>
    </div>
</template>

<script>
import FlashcardAPI from '../../Api/FlashcardAPI.js'

export default {
    data () {
        return {
            flashcards: [],
            info: "Not Run Yet"
        }
    },
    methods: {
        getFlashcards() {

        }
    },
    created () {
        this.info = "Getting Flashcards";
        FlashcardAPI
            .AllFlashcards()
            .then((data) => {
                this.flashcards = data;
                this.info = "Done";
            });
    }
}
//# sourceURL=settings.vue
</script>
