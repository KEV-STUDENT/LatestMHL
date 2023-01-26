<template>
    <div>
        <button @click="increment">Increment</button>
        <button @click="decrement">Decriment</button>
    </div>
    <div> Тест работает !!! Count = {{ count }}</div>
</template>
<script>
import { defineSSRCustomElement } from 'vue';

export default{
    data(){
       return { count : 0}
    },
    methods:{
        increment(){
            this.count += 1
        },
        decrement(){
            this.count -= 1
        }
    }
}
</script>
<style></style>