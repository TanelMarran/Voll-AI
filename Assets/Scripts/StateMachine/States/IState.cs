using UnityEngine.PlayerLoop;
using UnityEngine;

namespace Movement
{
    public abstract class State<T>
    {
        public T Handler;
        public int Index;

        protected State(T handler, int index)
        {
            Handler = handler;
            Index = index;
        }

        public virtual void Start(){
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }
        
        public override string ToString()
        {
            return this.GetType().Name;
        }

        public int getIndex()
        {
            return Index;
        }
    }
}