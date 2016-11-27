using UnityEngine;

[System.Serializable]
public class GenerationData
{
    [SerializeField]
    [Range(0, 100)]
    private float _rotLeftChanse;
    [SerializeField]
    [Range(0, 100)]
    private float _rotRightChanse;
    [SerializeField]
    [Range(0, 100)]
    private float _reverseChanse;
    [SerializeField]
    [Range(0,100)]
    private float _straightChanse;

    //Convert Priority to Chance;
    public float RotLeftChanse
    {
        get
        {
            float total = _rotLeftChanse + _rotRightChanse + _reverseChanse + _straightChanse;
            return(_rotLeftChanse / total) * 100;
        }
    }
    public float RotRightChanse
    {
        get
        {
            float total = _rotLeftChanse + _rotRightChanse + _reverseChanse + _straightChanse;
            return RotLeftChanse + (_rotRightChanse / total) * 100;
        }
    }
    public float ReverseChanse
    {
        get
        {
            float total = _rotLeftChanse + _rotRightChanse + _reverseChanse + _straightChanse;
            return RotRightChanse +(_reverseChanse / total) * 100;
        }
    }
    public float StraightChanse
    {
        get
        {
            float total = _rotLeftChanse + _rotRightChanse + _reverseChanse + _straightChanse;
            return ReverseChanse + (_straightChanse / total) * 100;
        }
    }
}
