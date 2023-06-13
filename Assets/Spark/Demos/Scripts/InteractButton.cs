using UnityEngine;

namespace LeastSquares
{
    public class InteractButton : MonoBehaviour
    {
        public BarkAIDialogue Dialogue;

        public void Talk()
        {
            Dialogue.Talk();
        }
    }
}