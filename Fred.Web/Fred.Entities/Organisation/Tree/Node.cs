using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.Entities.Organisation.Tree
{
    /// <summary>
    /// Represent un noeud
    /// </summary>
    /// <typeparam name="T">Type contenu dans le noeud</typeparam>
    [DebuggerDisplay("Data = {Data} Parent = {Parent}")]
    public class Node<T>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="data">L'object</param>
        public Node(T data)
        {
            this.Data = data;
            this.Children = new List<Node<T>>();
        }

        /// <summary>
        /// L'object contenue dans le noeud
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// Le pere
        /// </summary>
        public Node<T> Parent { get; private set; }

        /// <summary>
        /// Les enfants
        /// </summary>
        public List<Node<T>> Children { get; private set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>La representation de l'object contenu</returns>
        public override string ToString()
        {
            return Data.ToString();
        }

        /// <summary>
        /// Rajoute un enfant
        /// </summary>
        /// <param name="data">La donnée du noeud</param>
        public void AddChild(Node<T> data)
        {
            data.Parent = this;
            this.Children.Add(data);
        }

        /// <summary>
        /// Rajoute des enfants
        /// </summary>
        /// <param name="children">Les donnees du noeud</param>
        public void AddAllChildren(List<Node<T>> children)
        {
            foreach (Node<T> child in children)
            {
                child.Parent = this;
            }

            this.Children.AddRange(children);
        }

        /// <summary>
        /// Dertermine si c'est un noeud sans enfant
        /// </summary>
        /// <returns>vrais si pas d'enfant</returns>
        public bool IsLeaf()
        {
            return this.Children == null || this.Children.Count == 0;
        }

        /// <summary>
        /// Du root au enfants, de root au enfants branche par branche
        /// </summary>
        /// <returns>Liste de nodes</returns>
        public List<T> PreOrder()
        {
            List<T> list = new List<T>();
            list.Add(Data);
            foreach (Node<T> child in Children)
            {
                list.AddRange(child.PreOrder());
            }
            return list;
        }

        /// <summary>
        /// Du root au enfants, de root au enfants branche par branche
        /// </summary>
        /// <param name="action">Action a effectuer sur chaque noeud</param>
        /// <returns>Liste de nodes</returns>
        public List<T> PreOrder(Action<T> action)
        {
            List<T> list = new List<T>();
            list.Add(Data);
            action(Data);
            foreach (Node<T> child in Children)
            {
                list.AddRange(child.PreOrder(action));
            }
            return list;
        }


        /// <summary>
        /// Des enfants au parents, niveau par niveau
        /// </summary>
        /// <returns>Liste de nodes</returns>
        public List<T> PostOrder()
        {
            List<T> list = new List<T>();
            foreach (Node<T> child in Children)
            {
                list.AddRange(child.PostOrder());
            }

            list.Add(Data);
            return list;
        }

        /// <summary>
        /// Des root au enfants niveau par niveau
        /// </summary>
        /// <param name="selector">selecteur d'object</param>
        /// <returns>Liste de nodes</returns>
        public List<T> LevelOrder(Predicate<T> selector = null)
        {
            List<T> list = new List<T>();
            Queue<Node<T>> queue = new Queue<Node<T>>();
            queue.Enqueue(this);
            while (queue.Count != 0)
            {
                Node<T> temp = queue.Dequeue();
                foreach (Node<T> child in temp.Children)
                {

                    queue.Enqueue(child);

                }
                if (selector == null)
                {
                    list.Add(temp.Data);
                }
                else
                {
                    if (selector(temp.Data))
                    {
                        list.Add(temp.Data);
                    }
                }


            }
            return list;
        }

        /// <summary>
        /// Retourne les parents
        /// </summary>
        /// <returns>Liste de parents</returns>
        public List<T> Parents()
        {
            List<T> list = new List<T>();
            if (Parent != null)
            {
                list.Add(Parent.Data);
                list.AddRange(Parent.Parents());
            }
            return list;
        }

        /// <summary>
        /// Donne la profondeur du noeud
        /// </summary>
        /// <returns>la profondeur du noeud</returns>
        public int Depth()
        {
            int deepest = 0;
            foreach (Node<T> child in Children)
            {
                int depth = 1 + child.Depth();
                if (deepest < depth)
                {
                    deepest = depth;
                }
            }
            return deepest;
        }
    }

}
