using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
        for(int i = 0; i < cols.Length; ++i) {
            Rigidbody targetR = cols[i].GetComponent<Rigidbody>();
            if(!targetR)
                continue;
            targetR.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetH = targetR.GetComponent<TankHealth>();
            if(!targetH)
                continue;
            targetH.TakeDamage(CalculateDamage(targetR.position));
        }

        m_ExplosionParticles.transform.parent = null;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 expToTarget = targetPosition - transform.position;
        float expDist = expToTarget.magnitude;
        float relativeDist = (m_ExplosionRadius - expDist) / m_ExplosionRadius;
        float damage = relativeDist * m_MaxDamage;
        return Mathf.Max(0f, damage);
    }
}