using UnityEngine;

namespace FayvitBasicTools
{
    public interface IFadeView
    {
        public bool Darken { get; set; }
        public bool Lighten { get; set; }
        public void ClearFade();
        public void StartFadeOut(Color fadeColor = default);
        public void StartFadeOutWithAction(System.Action acao, Color fadeColor = default);
        public void StartFadeOutWithAction(System.Action acao, float darkenTime, Color fadeColor = default);
        public void StartFadeInWithAction(System.Action acao, Color fadeColor = default);
        public void StartFadeInWithAction(System.Action acao, float lightenTime, Color fadeColor = default);
        public void StartFadeIn(Color fadeColor = default);
    }
}
