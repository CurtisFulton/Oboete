import Vue from 'vue'
import VueRouter from 'vue-router'
import App from './App.vue'

import Login from './Components/Login/Login.vue'
import FlashcardList from './Components/Flashcard/FlashcardList.vue'
import FlashcardDetails from './Components/Flashcard/FlashcardDetails.vue'

Vue.config.productionTip = false
Vue.use(VueRouter)

const routes = [
  { path: '/', component: App }, 
  { path: '/Flashcard/:id', component: FlashcardDetails, props: true },
  { path: '/Flashcard', component: FlashcardList },
  { path: '/Login', component: Login }
]

const router = new VueRouter({
  routes,
  mode: 'history'
})

new Vue({
  el: '#app',
  template: "<div><router-view></router-view></div>",
  router
})