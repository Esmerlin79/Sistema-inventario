<template>
    <v-layout align-start>
        <v-flex>
            <v-toolbar flat color="white">
                <v-btn @click="crearPDF"><v-icon>print</v-icon></v-btn>
                <v-toolbar-title>Articulos</v-toolbar-title>
                    <v-divider
                    class="mx-2"
                    inset
                    vertical
                    ></v-divider>
                    <v-spacer></v-spacer>
                    <v-text-field class="text-xs-center" v-model="search" append-icon="search" label="Búsqueda" single-line hide-details></v-text-field>
                    <v-spacer></v-spacer>
                    <v-dialog v-model="dialog" max-width="500px">
                    <v-btn slot="activator" color="primary" dark class="mb-2">Nuevo</v-btn>
                        <v-card>
                            <v-card-title>
                            <span class="headline">{{ formTitle }}</span>
                            </v-card-title>
                
                            <v-card-text>
                            <v-container grid-list-md>
                                <v-layout wrap>
                                     <v-flex xs6 sm6 md6>
                                        <v-text-field v-model="codigo" label="Codigo"></v-text-field>
                                    </v-flex>
                                    <v-flex xs6 sm6 md6>
                                        <v-select v-model="idcategoria" :items="categorias" label="Categoria">
                                        </v-select>
                                    </v-flex>
                                    <v-flex xs12 sm12 md12>
                                        <v-text-field v-model="nombre" label="Nombre"></v-text-field>
                                    </v-flex>
                                    <v-flex xs6 sm6 md6>
                                        <v-text-field  type="number" v-model="stock" label="Stock"></v-text-field>
                                    </v-flex>
                                    <v-flex xs6 sm6 md6>
                                        <v-text-field type="number" v-model="precio_venta" label="Precio Venta"></v-text-field>
                                    </v-flex>
                                    <v-flex xs12 sm12 md12>
                                        <v-text-field v-model="descripcion" label="Descripcion"></v-text-field>
                                    </v-flex> 
                                    <v-flex xs12 sm12 md12 v-show="valida">
                                        <div class="red--text" v-for="v in validaMensaje" :key="v" v-text="v">
                                            <!-- {{v}} -->
                                        </div>
                                    </v-flex> 
                                </v-layout>
                            </v-container>
                            </v-card-text>
                
                            <v-card-actions>
                            <v-spacer></v-spacer>
                            <v-btn color="blue darken-1" flat @click.native="close">Cancelar</v-btn>
                            <v-btn color="blue darken-1" flat @click.native="guardar">Guardar</v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-dialog>
                    <v-dialog v-model="adModal" max-width="350px">
                        <v-card>
                            <v-card-title class="headline" v-if="adAccion==1">Activar Articulo?</v-card-title>
                            <v-card-title class="headline" v-if="adAccion==2">Desactivar Articulo?</v-card-title>
                            <v-card-text>
                                Estas a punto de
                                <span v-if="adAccion==1">Activar</span>
                                <span v-if="adAccion==2">Desactivar</span>
                                el articulo {{adNombre}}
                            </v-card-text>
                            <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn color="red darken-2"  flat="flat" @click="ActivarDesactivarCerrar">
                                    Cancelar
                                </v-btn>
                                 <v-btn v-if="adAccion==1" color="blue darken-4" @click="activar" flat="flat">
                                    Activar
                                </v-btn>
                                <v-btn v-if="adAccion==2" color="blue darken-4" @click="desactivar" flat="flat">
                                    Desactivar
                                </v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-dialog>
                </v-toolbar>
            <v-data-table
                :headers="headers"
                :items="articulos"
                :search="search"
                class="elevation-1"
            >
                <template slot="items" slot-scope="props">
                    <td class="justify-center layout px-0">
                        <v-icon
                        small
                        class="mr-2"
                        @click="editItem(props.item)"
                        >
                        edit
                        </v-icon>
                        <template v-if="props.item.condicion">
                            <v-icon
                            small
                            @click="activarDesactivarMostrar(2,props.item)"
                            >
                            block
                            </v-icon>
                        </template>
                        <template v-else>
                            <v-icon
                            small
                            @click="activarDesactivarMostrar(1,props.item)"
                            >
                            check
                            </v-icon>
                        </template>
                </td>
                <td>{{ props.item.codigo }}</td>
                <td>{{ props.item.nombre }}</td>
                <td>{{ props.item.categoria }}</td>
                <td>{{ props.item.stock }}</td>
                <td>{{ props.item.precio_venta }}</td>
                <td >{{ props.item.descripcion }}</td>
                <td >
                    <div v-if="props.item.condicion">
                        <span class="blue--text">Activo</span>
                    </div>
                    <div v-else>
                        <span class="red--text">Inactivo</span>
                    </div>
                    </td>
                </template>
                <template slot="no-data">
                <v-btn color="primary" @click="listar">Resetear</v-btn>
                </template>
            </v-data-table>
        </v-flex>
    </v-layout>
</template>
<script>
import axios from 'axios'
import jsPDF from 'jspdf'
import autoTable from 'jspdf-autotable'
    export default {
        data(){
            return {
                articulos:[],
                dialog: false,
                headers: [
                { text: 'Opciones', value: 'opciones', sortable: false },
                { text: 'Codigo', value: 'codigo', sortable: false },
                { text: 'Nombre', value: 'nombre' },
                { text: 'Categoria', value: 'categoria' },
                 { text: 'Stock', value: 'stock', sortable: false },
                { text: 'Precio Venta', value: 'precio_venta',sortable: false},
                { text: 'Descripcion', value: 'descripcion',sortable: false},
                { text: 'Estado', value: 'condicion',sortable: false },
                ],
                search: '',
                editedIndex: -1,
                id:'',
                idcategoria:'',
                categorias:[],
                codigo:"",
                nombre:'',
                stock:0,
                precio_venta:0,
                descripcion:'',
                valida:0,
                validaMensaje:[],
                adModal:0,
                adAccion:0,
                adNombre:'',
                adId:'',
               
            }
        },
        computed: {
            formTitle () {
                return this.editedIndex === -1 ? 'Nuevo Articulo' : 'Editar Articulo'
            }
        },
        watch: {
            dialog (val) {
            val || this.close()
            }
        },
        created () {
            this.listar()      
            this.select()      
        },
        methods:{
            crearPDF(){
                var columns = [
                    {title: "Nombre", dataKey: "nombre"},
                    {title: "Código", dataKey: "codigo"}, 
                    {title: "Categoría", dataKey: "categoria"}, 
                    {title: "Stock", dataKey: "stock"},
                    {title: "Precio Venta", dataKey: "precio_venta"}
                ];
                var rows = [];

                this.articulos.map(function(x){
                    rows.push({nombre:x.nombre,codigo:x.codigo,categoria:x.categoria,stock:x.stock,precio_venta:x.precio_venta});
                });

                // Only pt supported (not mm or in)
                var doc = new jsPDF('p', 'pt');
                doc.autoTable(columns, rows, {
                    margin: {top: 60},
                    addPageContent: function(data) {
                        doc.text("Listado de Artículos", 40, 30);
                    }
                });
                doc.save('Articulos.pdf');
            },
            listar(){
                let me=this;
                let header={"Authorization" :" Bearer "+this.$store.state.token};
                let configuracion = {headers : header}
                axios.get('Articulos/Listar',configuracion).then(function(response){
                    me.articulos = response.data;
                   // console.log(response)
                }).catch(function(error){
                    console.log(error)
                })
            },
              select(){
                let me=this;
                let categoriaArray = []
                let header={"Authorization" :" Bearer "+this.$store.state.token};
                let configuracion = {headers : header}
                axios.get('Categorias/Select',configuracion).then(function(response){
                    categoriaArray = response.data;
                    // for(let item in categoriaArray){
                    //     me.categorias.push({text:categoriaArray[item].nombre,value:categoriaArray[item].idcategoria})
                    // }
                    categoriaArray.map(function(x){
                        me.categorias.push( { text: x.nombre, value: x.idcategoria })
                    });
                }).catch(function(error){
                    console.log(error)
                })
            },

            editItem (item) {
                this.id = item.idarticulo;
                this.idcategoria = item.idcategoria
                this.codigo = item.codigo
                this.nombre = item.nombre;
                this.stock = item.stock;
                this.precio_venta = item.precio_venta;
                this.descripcion = item.descripcion;
                this.editedIndex = 1
                this.dialog = true
            },

            close () {
                this.dialog = false
                this.limpiar()
            },
            limpiar(){
                this.id=''
                this.idcategoria =''
                this.nombre =''
                this.codigo =''
                this.precio_venta =''
                this.stock =''
                this.descripcion = ''
                this.editedIndex = -1
            },

            guardar () {
                if(this.validar()){
                    console.log(this.validaMensaje)
                    return;
                }
                if (this.editedIndex > -1) {
                    //Edit
                    let me = this;
                    let header={"Authorization" :" Bearer "+this.$store.state.token};
                    let configuracion = {headers : header}
                   axios.put('Articulos/Actualizar',{
                       "idarticulo":me.id,
                       "idcategoria":me.idcategoria,
                       "codigo":me.codigo,
                       "nombre":me.nombre,
                       "precio_venta":me.precio_venta,
                       "stock":me.stock,
                       "descripcion":me.descripcion
                   },configuracion).then(function(response){
                       me.close()
                       me.listar()
                       me.limpiar()
                   }).catch(function(errors){
                       console.log(errors)
                   })

                } else {
                    let me = this;
                    let header={"Authorization" :" Bearer "+this.$store.state.token};
                    let configuracion = {headers : header}
                   axios.post('Articulos/Crear',{
                      "idcategoria":me.idcategoria,
                       "codigo":me.codigo,
                       "nombre":me.nombre,
                       "precio_venta":me.precio_venta,
                       "stock":me.stock,
                       "descripcion":me.descripcion
                   },configuracion).then(function(response){
                       me.close()
                       me.listar()
                       me.limpiar()
                   }).catch(function(errors){
                       console.log(errors)
                   })
                }
            },
            validar(){
                this.valida = 0;
                this.validaMensaje = []
                
                if(this.nombre.length < 3 || this.nombre.length > 50){
                    this.validaMensaje.push("El Nombre no debe tener más de 50 caracteres, ni menos de 3.");
                }
                if(!this.idcategoria){
                    this.validaMensaje.push("Seleccione una categoria.");
                }
                if(!this.stock || this.stock == 0){
                    this.validaMensaje.push("Ingrese el stock inicial del articulo.");
                }
                if(!this.precio_venta || this.precio_venta == 0){
                    this.validaMensaje.push("Ingrese el precio de venta del articulo.");
                }
                if(this.validaMensaje.length){
                    this.valida = 1;
                }
                return this.valida
            },
            activarDesactivarMostrar(accion,obj){
                this.adModal=1
                this.adNombre = obj.nombre
                this.adId = obj.idarticulo
                if(accion == 1){
                    this.adAccion = 1;
                }else if(accion == 2){
                    this.adAccion = 2;  
                }else{
                    this.adModal=0
                }
            },
            activar(){
                let me = this;
                let header={"Authorization" :" Bearer "+this.$store.state.token};
                let configuracion = {headers : header}
                axios.put('Articulos/Activar/'+me.adId,{},configuracion).then(function(response){
                    me.adModal = 0;
                    me.adAccion = 0;
                    me.adNombre =''
                    me.adId=''
                    me.listar()
                }).catch(function(errors){
                    console.log(errors)
                })
            },
            desactivar(){
                let me = this;
                let header={"Authorization" :" Bearer "+this.$store.state.token};
                let configuracion = {headers : header}
                axios.put('Articulos/Desactivar/'+me.adId,{},configuracion).then(function(response){
                    me.adModal = 0;
                    me.adAccion = 0;
                    me.adNombre =''
                    me.adId=''
                    me.listar()
                }).catch(function(errors){
                    console.log(errors)
                })
            },
            ActivarDesactivarCerrar(){
               this.adModal = 0
            }
        }        
    }
</script>
