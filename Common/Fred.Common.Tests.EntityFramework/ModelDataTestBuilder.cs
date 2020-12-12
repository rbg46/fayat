using Fred.DesignPatterns.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Common.Tests.EntityFramework
{
    public class ModelDataTestBuilder<T> : ModelDataBuilder<T> where T : class, new()
    {
        /// <summary>
        /// Construit une liste d'entite
        /// </summary>
        /// <typeparam name="T">entite a cloner</typeparam>
        /// <param name="numberOfObjects">le nombre d'objets voulus</param>
        /// <returns></returns>
        public virtual IEnumerable<T> BuildNObjects(int numberOfObjects)
        {
            return Enumerable.Repeat(new T(), numberOfObjects) as IEnumerable<T>;
        }

        /// <summary>
        /// Construit une liste de nouvelles entites
        /// </summary>
        /// <typeparam name="T">entite a cloner</typeparam>
        /// <param name="numberOfObjects">le nombre d'objets voulus</param>
        /// <returns></returns>
        public virtual IEnumerable<T> BuildNObjects(int numberOfObjects, bool copyEntite)
        {
            return Enumerable.Repeat(copyEntite ? Model : new T(), numberOfObjects) as IEnumerable<T>;
        }

        /// <summary>
        /// Construit une liste d'entite
        /// </summary>
        /// <typeparam name="T">entite a cloner</typeparam>
        /// <param name="numberOfObjects">le nombre d'objets voulus</param>
        /// <returns></returns>
        public virtual IEnumerable<T> BuildNObjects(int numberOfObjects, T entite)
        {
            return Enumerable.Repeat(entite, numberOfObjects) as IEnumerable<T>;
        }

        /// <summary>
        /// Construit une liste d'entite
        /// </summary>
        /// <typeparam name="T">entite a cloner</typeparam>
        /// <param name="numberOfObjects">le nombre d'objets voulus</param>
        /// <returns></returns>
        public virtual IEnumerable<T> BuildNObjects(int numberOfObjects, ModelDataTestBuilder<T> entityBuilder)
        {
            return Enumerable.Repeat(entityBuilder.Model, numberOfObjects) as IEnumerable<T>;
        }

        /// <summary>
        /// Construit une liste d'entite
        /// </summary>
        /// <typeparam name="T">entite a cloner</typeparam>
        /// <param name="numberOfObjects">le nombre d'objets voulus</param>
        /// <returns></returns>
        public virtual IEnumerable<T> BuildNObjects<B>(int numberOfObjects, Func<B, T> funcBuilding) where B : ModelDataTestBuilder<T>, new()
        {
            return Enumerable.Repeat(funcBuilding.Invoke(this as B), numberOfObjects) as IEnumerable<T>;
        }

        /// <summary>
        /// Permet de construire un fakeDBSet pour un context
        /// </summary>
        /// <param name="numberOfObjects">nombre occurence</param>
        /// <param name="ent">object a dupliquer</param>
        /// <returns>FakeDbSet</returns>
        public FakeDbSet<T> BuildFakeDbSet(int numberOfObjects, T ent)
        {
            var set = new FakeDbSet<T>();
            set.AddRange(BuildNObjects(numberOfObjects, ent));
            return set;
        }

        /// <summary>
        /// Permet de construire un fakeDBSet pour un context
        /// </summary>
        /// <param name="numberOfObjects">nombre occurence</param>
        /// <returns>FakeDbSet</returns>
        public FakeDbSet<T> BuildFakeDbSet(int numberOfObjects)
        {
            var set = new FakeDbSet<T>();
            set.AddRange(BuildNObjects(numberOfObjects, true));
            return set;
        }


        /// <summary>
        /// Permet de construire un fakeDbSet 
        /// </summary>
        /// <param name="Objects">Serie d'objects</param>
        /// <returns>FakeDbSet</returns>
        public FakeDbSet<T> BuildFakeDbSet(params T[] Objects)
        {
            var set = new FakeDbSet<T>();
            foreach (var item in Objects)
            {
                set.Add(item);
            }
            return set;
        }

        /// <summary>
        /// Permet de construire un fakeDbSet 
        /// </summary>
        /// <param name="list">liste d'Entité</param>
        /// <returns>FakeDbSet</returns>
        public FakeDbSet<T> BuildFakeDbSet(IEnumerable<T> list)
        {
            var set = new FakeDbSet<T>();
            foreach (var item in list)
            {
                set.Add(item);
            }
            return set;
        }
    }
}
