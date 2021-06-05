using DG.Tweening;

public interface IFadeMotion
{
    Sequence Enter(float timescale = 1,bool activate = true);
    Sequence Exit(float timescale = 1,bool activate = true);
}