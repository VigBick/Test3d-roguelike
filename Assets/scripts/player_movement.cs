using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 checkpoint;
    public AudioSource[] jumpFX;
    public AudioSource[] bgmFX;
    public Animator animator;


    private CharacterController controller;

    private void Start() {
        controller = GetComponent<CharacterController>();
    }
    private void Update() {
        Move();//movimiento de personaje
        fall();//detectar caidas y reaparicion en checkpoint
    }

    private void Move(){
        Vector3 localScale = transform.localScale;
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);//Check si esta tocando el suelo
        if(isGrounded && velocity.y < 0) //Si no toca el suelo, caerá
        {
            velocity.y = -2f;
        }
        float movez = Input.GetAxis("Vertical");//movimiento en eje z
        float movex = Input.GetAxis("Horizontal");//movimiento en eje x
        moveDirection = new Vector3(movex,0,movez);//aplicar el movimiento al vector de movimiento

        if(movex < 0)//si se mueve a la izquierda, se invierte el sprite
        {
            localScale.x=-1;
        }
        else//si se mueve a la derecha, se coloca normalmente el sprite
        {
            localScale.x=1;
        }
        if(movex!=0 || movez!=0)//detección de movimiento para cambiar animacion
        {
            animator.SetFloat("speed", 1);//el parametro speed es uno, por lo que la animacion cambia
        }else{animator.SetFloat("speed", 0);}//al dejar de moverse se vuelve a la animacion idle
        transform.localScale = localScale;
        if(isGrounded)//Si no toca el suelo, no se puede mover ni saltar
        {
            if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))//check de caminar
            {
                walk();
            }   
            else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))//check de correr
            {
                run();
            }
            else if(moveDirection == Vector3.zero)//check sin movimiento
            {
                Idle();
            }

            if(Input.GetKeyDown(KeyCode.LeftAlt))//boton de salto
            {
                jump();
            }
        }
        moveDirection *= moveSpeed;
        controller.Move(moveDirection*Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle(){

    }
    private void walk(){
        moveSpeed = walkSpeed;
    }
    private void run(){
        moveSpeed = runSpeed;
    }

    private void jump(){
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        jumpFX[0].Play();
    }
    private void fall(){
        if(transform.position.y<-20)//si alcanza cierta altura negativa, el personaje es recolocado
        {
            transform.position = checkpoint;
        }
        if(transform.position.z > 80 && transform.position.y > 4 && transform.position.x> -10 && transform.position.x < 10)
        {//en caso de llegar al nivel 2, se cambia el área de checkpoint
            checkpoint = new Vector3(0,10, 85);
        }
    }
    //float speed = 6f;


    // Update is called once per frame
    /*void Update()
    {
        float horizontal =  Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool zaxis = Input.GetButton("Jump");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 jump = new Vector3(horizontal, 20, vertical).normalized;
        if(direction.magnitude >= 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
        if(zaxis)
        {
            controller.Move(jump * speed * Time.deltaTime);
        }else
        {
            controller.Move(-jump * speed * Time.deltaTime);
        }
    }*/
}
