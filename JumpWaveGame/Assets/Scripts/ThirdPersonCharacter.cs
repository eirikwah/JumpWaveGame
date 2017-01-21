using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
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

		void Start()
		{
			ChangeMaterialInChild(transform);

			gameObject.name = LayerMask.LayerToName(gameObject.layer);
			animator = GetComponent<Animator>();
			rigidbody = GetComponent<Rigidbody>();
			constantForce = GetComponent<ConstantForce>();
			rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
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
									

								Debug.Log(mesh.materials.Length);
//								mesh.sharedMaterial = playerMaterials[i];
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


		private void Update()
		{
			if(stompAttack)
			{
				
			}
		}

		public void ReceiveInput(Vector3 move, bool attack, bool jump)
		{
			if(Input.GetKeyDown(KeyCode.P))
			{
				GetComponent<Rigidbody>().AddForce(new Vector3(0, 8, 3), ForceMode.Impulse);
			}
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);

			CheckGroundStatus();

			move = Vector3.ProjectOnPlane(move, groundNormal);

			forwardAmount = move.z;

			Rotate(move);
			Vector3 v = rigidbody.velocity;
			// control and velocity handling is different when grounded and airborne:
			if (isGrounded)
			{
				HandleGroundedMovement(jump);
				
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
				HandleAirborneMovement();
			}

			// send input and other state parameters to the animator
			UpdateAnimator(move);

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


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
//			groundCheckDistance = rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}


		void HandleGroundedMovement(bool jump)
		{
			// check whether conditions are right to allow a jump:
			if (jump && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
				// jump!
//				rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpPower, rigidbody.velocity.z);
				rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
				isGrounded = false;
//				groundCheckDistance = 0.1f;
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
				}
				else
				{
					stompAttack = true;
					rigidbody.velocity = Vector3.zero;
					rigidbody.AddForce(Vector3.down * stompForce, ForceMode.Impulse);
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
						Debug.Log(gameObject.name + " stomps the ground!");
					}
				}

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
