<template>
    <div id="home">
        <h1>Hello World!</h1>
        <router-link to="/Login" exact>Login</router-link>
        <router-link to="/Flashcard" exact>Flashcard List</router-link>
        <router-view></router-view>
    </div>
</template>

<script>
export default { }
</script>

<style lang="scss">

@import './variables.scss';

#home {
    max-width: 100%;
    color: $vue-blue;
}

h1 {
    text-align: center;
}

</style>