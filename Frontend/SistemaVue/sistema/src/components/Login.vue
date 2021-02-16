<template>
    <v-layout align-center justify-center>
        <v-flex xs12 sm8 md6 lg5 x14>
            <v-card>
                <v-toolbar dark color="blue darken-3">
                    <v-toolbar-title>
                        Acceso al Sistema
                    </v-toolbar-title>
                </v-toolbar>
                <v-card-text>
                    <v-text-field autofocus color="accent" @keyup.enter="ingresar" v-model="email" label="Email" required>
                    </v-text-field>
                    <v-text-field type="password" color="accent" @keyup.enter="ingresar" v-model="password" label="Pasword" required>
                    </v-text-field>
                    <v-flex class="red--text" v-if="error">
                        {{error}}
                    </v-flex>
                </v-card-text>
                <v-card-actions class="px-3 pb-3">
                    <v-flex text-xs-right>
                        <v-btn color="primary" @click="ingresar">Ingresar</v-btn>
                    </v-flex>
                </v-card-actions>
            </v-card>
        </v-flex>
    </v-layout>
</template>

<script>
import axios from 'axios'
export default {
    data(){
        return{
            email:'',
            password:'',
            error:null
        }
    },
    methods:{
        ingresar(){
            this.error = null
            axios.post('Usuarios/Login',{email:this.email,password:this.password})
                .then(response => {
                   console.log(response.data)
                    return response.data
                }).then(data =>{
                    this.$store.dispatch("guardarToken",data.token)
                    this.$router.push({name:"home"})
                }).catch(error =>{
                    if(error.response.status == 400){
                        this.error = "No es un email valido"
                    }else if(error.response.status == 404){
                         this.error = "No existe el usuario o sus datos son incorrectos"
                    }else{
                         this.error = "Ocurrio un error"
                    }
                    // console.log(error)
                })
        }
    }
}
</script>