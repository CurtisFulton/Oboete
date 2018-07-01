<template>
    <div id="app">
        <h1>Flashcard</h1>
        <p>Status: {{ info }}</p>
        <div>{{flashcard}}</div>
    </div>
</template>

<script>
import FlashcardAPI from '../../Api/FlashcardAPI.js'

export default {
    props: ['id'],
    data () {
        return {
            flashcard : null,
            info: "Not Run Yet"
        }
    },
    methods: {
        getFlashcard(id) {

        }
    },
    created () {
        this.info = "Getting Flashcard";
        
        FlashcardAPI
            .GetFlashcard(this.id)
            .then((data) => {
                this.flashcard = data;
                this.info = "Done";
            });
    }
}
//# sourceURL=settings.vue
</script>
