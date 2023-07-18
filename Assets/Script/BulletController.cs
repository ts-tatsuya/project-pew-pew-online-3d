using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletController : MonoBehaviourPunCallbacks, IPunObservable
{

    [Header("Bullet Config")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float timeOut;
    [SerializeField]
    private int bulletPower;

    void Start()
    {
        Destroy(gameObject, timeOut);
        //photonView.RPC("RPC_SetSelfDestruct", RpcTarget.AllBuffered);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.GetComponent<PlayerController>() is PlayerController player)
        {
            player.healthPoint -= bulletPower;
            photonView.RPC("RPC_SelfDestruct", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void RPC_SelfDestruct()
    {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            transform.position = (Vector3)stream.ReceiveNext();
            Quaternion rotationTemp = (Quaternion)stream.ReceiveNext();
            transform.rotation = new Quaternion(transform.rotation.x, rotationTemp.y, transform.rotation.z, 1);
        }
    }
}
