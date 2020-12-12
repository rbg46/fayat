using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.DesignPatterns.Builder
{
    public class ModelDataBuilder<T> where T : class, new()
    {
        private T model;
        /// <summary>
        /// obtient et définit l'entité
        /// </summary>
        public T Model
        {
            get
            {
                return model != null ? model : (model = New());
            }
            set
            {
                model = value;
            }
        }

        private IEnumerable<PropertyInfo> propertiesInfos;
        /// <summary>
        /// Obtient la liste des propriétés à l'exécution de la classe
        /// </summary>
        protected IEnumerable<PropertyInfo> PropertiesInfos => propertiesInfos ?? (propertiesInfos = typeof(T).GetRuntimeProperties());

        /// <summary>
        /// Execute une action fluent
        /// </summary>
        /// <param name="action"></param>
        /// <returns>fluent</returns>
        public virtual ModelDataBuilder<T> Do(Action<T> action)
        {
            action.Invoke(Model);
            return this;
        }

        /// <summary>
        /// Excecute une crétaion d'instance de l'entite
        /// </summary>
        /// <returns></returns>
        public virtual T New()
        {
            Model = new T();
            //
            return Model;
        }

        /// <summary>
        /// Contruit l'entité
        /// </summary>
        /// <returns>entité</returns>
        public virtual T Build()
        {
            T buildResult = Model;
            model = null;
            return buildResult;
        }

        /// <summary>
        /// Permet d'affecter une valeur à un champ
        /// </summary>
        /// <param name="field">nom du champ</param>
        /// <param name="value">veleur du champ</param>
        /// <returns>test builder</returns>
        public virtual ModelDataBuilder<T> SetFieldValue(string field, object value)
        {
            //get also non-public properties
            var prop = PropertiesInfos.FirstOrDefault(c => c.Name == field);

            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(Model, value, null);
            }

            return this;
        }
    }
}
