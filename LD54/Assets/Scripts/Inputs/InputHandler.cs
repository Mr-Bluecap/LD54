namespace Inputs
{
    public abstract class InputHandler
    {
        protected InputManager InputManager;
        
        public InputHandler(InputManager inputManager)
        {
            InputManager = inputManager;
        }

        public abstract void OnMouseHover();
        
        
        public abstract void OnLeftMouseDown();

        public abstract void OnLeftMouseHold();

        public abstract void OnLeftMouseUp();
    }
}