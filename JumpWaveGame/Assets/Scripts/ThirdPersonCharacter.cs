using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		public bool gameOver;

		[SerializeField] 
		private float runSpeed = 1;

		[SerializeField] 
		float jumpPower = 12f;

		[SerializeField] 
		private float attackForce = 30;

		[SerializeField] 
		private float stompForce = 40;

		[SerializeField] 
		private float groundCheckDistance = 0.1f;

		[SerializeField] 
		private GameObject attackCollider;

		[SerializeField]
		private List<Material> playerMaterials;

		private Rigidbody rigidbody;
		private ConstantForce constantForce;
		private Animator animator;
		private bool isGrounded;
		private float forwardAmount;
		private Vector3 groundNormal;

		private bool stompAttack;
		private bool stompQueued;
		private bool hasJumped;
		private bool hasDoubleJumped;

		private float randomIdleFloat = 1;

		[FMODUnity.EventRef]
		public string KickSound = "event:/KickMiss";

		[FMODUnity.EventRef]
		public string StompSound = "event:/KickHit";

		[FMODUnity.EventRef]
		public string JumpSound = "event:/Jump";

		void Start()
		{
			ChangeMaterialInChild(transform);
			attackCollider.SetActive(false);
			gameObject.name = LayerMask.LayerToName(gameObject.layer);
			animator = GetComponent<Animator>();
			rigidbody = GetComponent<Rigidbody>();
			constantForce = GetComponent<ConstantForce>();
			rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			InvokeRepeating("AnimationCycleLoop", 1, 0.5f);
		}

		private void ChangeMaterialInChild(Transform t)
		{
			foreach(Transform child in t)
			{
				MeshRenderer mesh = child.GetComponent<MeshRenderer>();
				SkinnedMeshRenderer skinMesh = child.GetComponent<SkinnedMeshRenderer>();
				if(mesh != null)
				{
					if(mesh.gameObject.name.Contains("HIGHLIGHT") || mesh.gameObject.name.Contains("SCLERA"))
					{

					}
					else
					{
						for(int i = 0; i < playerMaterials.Count; i++)
						{
							if(playerMaterials[i].name == gameObject.name)
							{
								mesh.materials[0].color = playerMaterials[i].color;
								if(mesh.materials.Length == 2)
								mesh.materials[1].color = playerMaterials[i].color;

							}
						}
						
					}
				}

				if (skinMesh != null)
				{
					for (int i = 0; i < playerMaterials.Count; i++)
					{
						if (playerMaterials[i].name == gameObject.name)
						{
							skinMesh.materials[0].color = playerMaterials[i].color;
							if (skinMesh.materials.Length == 2)
								skinMesh.materials[1].color = playerMaterials[i].color;
						}
					}
				}

				ChangeMaterialInChild(child);
			}
		}

		public void ReceiveInput(Vector3 move, bool attack, bool jump)
		{
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);

			CheckGroundStatus();

			move = Vector3.ProjectOnPlane(move, groundNormal);

			forwardAmount = move.z;

			Rotate(move);
			Vector3 v = rigidbody.velocity;

			JumpAction(jump);

			UpdateAnimator(move);

			if(gameOver)
			{
				return;
			}

			if(isGrounded)
			{
				if(forwardAmount > 0.1f)
				{
					if(rigidbody.velocity.magnitude < 7)
					v = rigidbody.velocity + transform.forward * forwardAmount * runSpeed;
//					rigidbody.velocity += transform.forward*forwardAmount*runSpeed;
					rigidbody.velocity = v;
					//					if(rigidbody.velocity.magnitude < runSpeed)
					//						rigidbody.AddRelativeForce(Vector3.forward * runSpeed, ForceMode.VelocityChange);
					//						constantForce.relativeForce = move * 1000;
					//					transform.Translate(Vector3.forward * runSpeed);
				}
			}
			else
			{
			}

			// send input and other state parameters to the animator

			AttackAction(attack);
		}

		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
			animator.SetBool("OnGround", isGrounded);

			if (!isGrounded)
			{
				animator.SetFloat("Jump", rigidbody.velocity.y);
			}
		}

		void JumpAction(bool jump)
		{
			// check whether conditions are right to allow a jump:
			if(jump && isGrounded)
			{
				// jump!
				//				rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpPower, rigidbody.velocity.z);
				rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
				isGrounded = false;
				//				groundCheckDistance = 0.1f;
				hasJumped = true;
				RuntimeManager.PlayOneShot(JumpSound, Vector3.zero);
			}
			else if(jump && !isGrounded && !hasDoubleJumped && !gameOver )
			{
				animator.SetTrigger("DoubleJump");
				rigidbody.velocity = Vector3.zero;
				rigidbody.AddRelativeForce(Vector3.up * (jumpPower / 1.1f) + Vector3.forward * (jumpPower * (forwardAmount / 2 )), ForceMode.VelocityChange);
				hasDoubleJumped = true;

				RuntimeManager.PlayOneShot(JumpSound, Vector3.zero);
			}
		}

		private void AttackAction(bool attack)
		{
			if(attack)
			{

				if(isGrounded)
				{
					rigidbody.AddRelativeForce(Vector3.forward * attackForce, ForceMode.Acceleration);
					animator.SetTrigger("Attack");
					attackCollider.SetActive(true);
					Invoke("StopAttack", 0.2f);
					RuntimeManager.PlayOneShot(KickSound, Vector3.zero);
				}
				else if(!isGrounded && hasJumped)
				{
					stompAttack = true;
					rigidbody.velocity = Vector3.zero;
					rigidbody.AddForce(Vector3.down * stompForce, ForceMode.Impulse);
					RuntimeManager.PlayOneShot(StompSound, Vector3.zero);
				}
			}
		}

		private void StopAttack()
		{
			attackCollider.SetActive(false);
		}

		private void Rotate(Vector3 input)
		{
			float rotation = Mathf.Atan2(input.x, input.z)*Mathf.Rad2Deg;
			transform.Rotate(new Vector3(0, rotation, 0));
		}

		public void OnAnimatorMove()
		{
			if (isGrounded)
			{
//				Vector3 v = rigidbody.velocity;
//
//				v = transform.forward * forwardAmount * runSpeed;
//				v.y = rigidbody.velocity.y;
//
//				rigidbody.velocity = v;
			}
		}

		public void StartWinningAnimation()
		{
			animator.SetTrigger("Cheer");
		}

		private void AnimationCycleLoop()
		{
			if(!animator.isInitialized)
				return;
//				Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
//			if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle" + randomIdleFloat) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
//			{
//				Debug.Log("dddddd");
//				randomIdleFloat = Random.Range(1, 11);
//                animator.SetFloat("IdleFloat", randomIdleFloat);
//			}

				animator.SetInteger("DanceNumber", Random.Range(1, 5));
			
		}

		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
//#if UNITY_EDITOR
//			// helper to visualise the ground check ray in the scene view
//			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
//#endif
//			// 0.1f is a small offset to start the ray from inside the character
//			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
			{
				groundNormal = hitInfo.normal;

				if(!isGrounded && stompAttack)
				{
					if(hitInfo.collider.tag == "Playfield")
					{
						FindObjectOfType<WaveCreator>().CreateWave(transform.position, gameObject.name);
						stompAttack = false;
					}
				}

				hasDoubleJumped = false;
				hasJumped = false;
				isGrounded = true;
			}
			else
			{
				isGrounded = false;
				groundNormal = Vector3.up;
			}
		}

//		void OnCollisionEnter(Collision col)
//		{
//			isGrounded = true;
//		}
//
//		void OnCollisionExit(Collision col)
//		{
//			isGrounded = false;
//		}
	}
}
