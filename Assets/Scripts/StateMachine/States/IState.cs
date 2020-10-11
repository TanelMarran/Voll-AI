using UnityEngine.PlayerLoop;
using UnityEngine;

namespace Movement
{
    public abstract class State<T>
    {
        public T Handler;

        protected State(T handler)
        {
            this.Handler = handler;
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
    }
}