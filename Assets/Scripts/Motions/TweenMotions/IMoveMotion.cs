using UnityEngine;

///何かを動かすためのインターフェイス
///脱Tweenを目指しているので返り値はbool
public interface IMoveMotion
{
    /// <summary>
    /// ステートとかではなく、ダメなときは入力を受け付けない
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    bool Move(Vector3 direction);
    /// <summary>
    /// ステートとかではなく、ダメなときは入力を受け付けない
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    bool Stop();
    bool Slide(Vector3 direction);
}