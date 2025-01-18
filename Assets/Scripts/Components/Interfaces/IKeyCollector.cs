using UnityEngine;

public interface IKeyCollector
{
    void Collect(Key key);
    void UseKey(string keyColor);
    bool HasKey(string keyColor);
}
