using AutoMapper;
using Sistema.Entidades.Almacen;
using Sistema.Web.Models.Almacen.Categoria;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.AMapper
{
    public class MapperHelper
    {
        private static MapperHelper _instance;

        private MapperHelper()
        {
            Mapper.Initialize(conf =>
            {
                conf.CreateMap<CategoriaViewModel, categoria>();
                conf.CreateMap<categoria, CategoriaViewModel>();
            });
        }

        public static MapperHelper Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = new MapperHelper();
                }
                return _instance;
            }
        }

        public To Map<From, To>(From obj)
        {
            return Mapper.Map<To>(obj);
        }

        public To Map<From, To>(From from, To to)
        {
            return Mapper.Map(from, to);
        }
    }
}
