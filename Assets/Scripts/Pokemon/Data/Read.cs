using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace Pokemon.Data
{
    /// <summary>
    /// Clase con los métodos necesarios para leer los datos de los ficheros json.
    /// </summary>
    public class Read
    {
        /// <summary>
        /// Obtiene el contenido de un fichero json.
        /// </summary>
        /// <param name="name">Nombre del archivo a leer.</param>
        /// <returns>Un string con el contenido del archivo.</returns>
        public static string Data(string name)
        {
            TextAsset txt = Resources.Load<TextAsset>("Database/"+name);
            return txt.text;
        }

        /// <summary>
        /// Convierte un objeto a un conjunto de pares clave-valor.
        /// </summary>
        /// <typeparam name="TValue">Tipo del contenido del diccionario.</typeparam>
        /// <param name="obj">Objeto cuyos atributos van a pasar al diccionario.</param>
        /// <returns>Un diccionario que contiene los atributos del objeto.</returns>
		public static Dictionary<string, TValue> ToDictionary<TValue>(object obj)
		{
			var json = JsonConvert.SerializeObject(obj);
			var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);
			return dictionary;
		}

        public static T Convert<T> (object obj)
        {
            string s = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(s);
        }
	}
}
