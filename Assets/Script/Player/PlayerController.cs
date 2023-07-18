using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CharacterController))]
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum PlayerAnimationState
    {
        Idle,
        Idle2,
        Idle3,
        Walk,
        Run,
        ShootInit,
        ShootCharge,
        ShootUnleash,
        ShootStop
    }
    [Header("Stats")]
    public int healthPoint;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotateSpeed;
    [Header("Shoot Setting")]
    [SerializeField]
    private float shootDuration;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform shootingPoint;
    private bool isRunning;
    private bool isShooting;
    private Animator animator;
    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isRunning = false;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthPoint <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        if (photonView.IsMine)
        {
            //photonView.RPC("Look", RpcTarget.AllBuffered);
            if (GameManager.IsUIFocused == false && Cursor.visible == false)
            {
                Look();
                if (isShooting == false)
                {
                    Move();
                    //photonView.RPC("Move", RpcTarget.AllBuffered);
                }
                if (Input.GetKeyDown(KeyCode.Mouse0) && isShooting == false)
                {
                    //Shoot();
                    photonView.RPC("RPC_Shoot", RpcTarget.AllBuffered);
                }
            }
        }

    }
    private void Move()
    {
        //Debug.Log(isForwardReversed);
        Vector3 moveValue = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        switch (isRunning)
        {
            case true:
                characterController.Move(moveValue * speed * 3 * Time.deltaTime);
                break;
            case false:
            default:
                characterController.Move(moveValue * speed * Time.deltaTime);
                break;
        }
        photonView.RPC("RPC_UpdateAnimation", RpcTarget.AllBuffered, MoveAnimationState(moveValue));
        //animator.Play(MoveAnimationState(moveValue).ToString());

    }
    private void Look()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 positionToLookAt = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.LookAt(new Vector3(positionToLookAt.x, transform.position.y, positionToLookAt.z));
    }
    [PunRPC]
    private void RPC_Shoot()
    {
        StartCoroutine(ShootProcess());
    }
    [PunRPC]
    private void RPC_UpdateAnimation(PlayerAnimationState animationState)
    {
        if (animator)
        {
            animator.Play(animationState.ToString());
        }
    }
    private IEnumerator ShootProcess()
    {
        photonView.RPC("RPC_UpdateAnimation", RpcTarget.AllBuffered, PlayerAnimationState.ShootInit);
        //animator.Play(PlayerAnimationState.ShootInit.ToString());
        isShooting = true;
        yield return new WaitUntil(() =>
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimationState.ShootUnleash.ToString());
        });
        Instantiate(
            bulletPrefab,
            shootingPoint.position,
            gameObject.transform.rotation);

        yield return new WaitUntil(() =>
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimationState.ShootStop.ToString());
        });
        isShooting = false;
    }
    private PlayerAnimationState MoveAnimationState(Vector3 moveVector)
    {

        if (moveVector.x != 0 || moveVector.z != 0)
        {
            switch (isRunning)
            {
                case true:
                    return PlayerAnimationState.Run;
                case false:
                default:
                    return PlayerAnimationState.Walk;
            }
        }
        else
        {
            return PlayerAnimationState.Idle;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

}
