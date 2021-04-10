using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    [Range(1, 50)]
    public float ShakeIntensity = 10f;
    [Range(0.1f, 5f)]
    public float ShakeTime = 0.5f;
    [Range(10, 50)]
    public float ShakeFrequency = 30;

    CinemachineBasicMultiChannelPerlin _cameraNoise;
    private float _shakeTimer, _startIntensity, _shakeTimerTotal;

    CinemachineVirtualCamera cam;
    float zoomedOutSize;
    float targetSize;
    float zoomSpeed;
    ProtoPlayer2D target;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Initialize(cam = GetComponent<CinemachineVirtualCamera>());
    }

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        ZoomTo(8, 0.5f);

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<ProtoPlayer2D>();
    }

    public void ZoomTo(int newSize, float speed)
    {
        targetSize = newSize;
        zoomSpeed = speed;
    }

    private void Update()
    {
        if (cam.m_Lens.OrthographicSize != targetSize)
        {
            cam.m_Lens.OrthographicSize = Mathf.MoveTowards(cam.m_Lens.OrthographicSize, targetSize, zoomSpeed * Time.deltaTime);
        }
    }

    public void Initialize(CinemachineVirtualCamera camera)
    {
        _cameraNoise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (!_cameraNoise) Debug.LogError("No CinemachineBasicMultiChannelPerlin found on camera!");
    }

    public void ShakeCamera(float intensity, float time)
    {
        if (!_cameraNoise)
        {
            Debug.LogWarning("No CinemachineBasicMultiChannelPerlin found on camera, Camerashake is disabled.");
            return;
        }

        _cameraNoise.m_FrequencyGain = ShakeFrequency;
        _cameraNoise.m_AmplitudeGain = intensity;
        _shakeTimer = time;
        _shakeTimerTotal = time;
    }

    void LateUpdate()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;

            if (_shakeTimer <= 0f)
            {
                _cameraNoise.m_AmplitudeGain = Mathf.Lerp(_startIntensity, 0, (1 - (_shakeTimer / _shakeTimerTotal)));
            }
        }
    }
}
