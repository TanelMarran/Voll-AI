using UnityEngine.PlayerLoop;

namespace Movement
{
    public abstract class State<T>
    {
        public T Handler;

        protected State(T handler)
        {
            this.Handler = handler;
        }

        public abstract void Update();

        public abstract void FixedUpdate();
        
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}