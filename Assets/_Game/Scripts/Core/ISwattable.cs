namespace PainterJourney.Core
{
    /// <summary>
    /// Something the brush stroke removes on touch instead of damaging, such as a paint blob.
    /// It lives in Core so the Player code can swat things without depending on the Enemies code.
    /// </summary>
    public interface ISwattable
    {
        /// <summary>Called when the brush's stroke touches it. Nobody takes damage.</summary>
        void Swat();
    }
}
