using UnityEngine;

public class AudioSpeaker : CharacterStats
{
    [SerializeField]
    GameObject particleEffect;
    [SerializeField]
    AudioClip shootSound;
    [SerializeField]
    AudioSource audioSource;

    public bool autoPlay = false;

    bool dead = false;
    private float playingDelta;


    [SerializeField, GetSet("isPlaying")]
    private bool _isPlaying = false;
    public bool isPlaying
    {
        get { return _isPlaying; }
        set
        {  
            if (value && !dead)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
            _isPlaying = value;
        }
    }

    public void Start()
    {
        particleEffect.SetActive(false);
        if (autoPlay)
        {
            isPlaying = true;
        }
    }

    public void OnDisable()
    {
        playingDelta = audioSource.time;
        particleEffect.SetActive(false);
        Debug.Log("Disabled");
    }

    public void OnEnable()
    {
        if(_isPlaying && !dead)
        {
            audioSource.time = playingDelta;
            audioSource.Play();
        }

    }

    public void PlayAudio(float delay = 0.0f)
    {
        if (!dead)
        {
            _isPlaying = true;
            audioSource.PlayDelayed(delay);
        }
    }


    protected override void Die()
    {
        if (dead)
            return;

        dead = true;
        particleEffect.SetActive(true);
        particleEffect.GetComponent<ParticleSystem>().Play();
        audioSource.Stop();
        _isPlaying = false;

        //Suppression de l'objet
        //Destroy(gameObject, 3.0f);
    }

    protected override void Hurt(float newAlpha)
    {
        if (!dead)
        {
            SoundManager.Instance.PlaySound(shootSound);
            this.Die();
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }


}