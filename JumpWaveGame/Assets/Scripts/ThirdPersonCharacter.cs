using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] private float runSpeed = 1;
		[SerializeField] float jumpPower = 12f;

		[SerializeField] private float attackForce = 30;

		[SerializeField] private float stompForce = 40;

		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.1f;

		Rigidbody m_Rigidbody;
		Animator m_Animator;
		bool m_IsGrounded;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		bool m_Crouching;


		void Start()
		{
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;

			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;
		}


		public void ReceiveInput(Vector3 move, bool attack, bool jump)
		{
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);

			CheckGroundStatus();

			move = Vector3.ProjectOnPlane(move, m_GroundNormal);

			m_TurnAmount = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;

			m_ForwardAmount = move.z;

			Rotate(move);

			// control and velocity handling is different when grounded and airborne:
			if (m_IsGrounded)
			{
				HandleGroundedMovement(jump);
				OnAnimatorMove();
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
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("OnGround", m_IsGrounded);

			if (!m_IsGrounded)
			{
				m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			}
		}


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}


		void HandleGroundedMovement(bool jump)
		{
			// check whether conditions are right to allow a jump:
			if (jump && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
				// jump!
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, jumpPower, m_Rigidbody.velocity.z);
				m_IsGrounded = false;
				m_Animator.applyRootMotion = false;
				m_GroundCheckDistance = 0.1f;
			}
		}

		private void AttackAction(bool attack)
		{
			if(attack)
			{
				m_Animator.SetTrigger("Attack");

				if(m_IsGrounded)
				{
					m_Rigidbody.AddRelativeForce(Vector3.forward * attackForce, ForceMode.Acceleration);

//					m_Rigidbody.velocity = m_Rigidbody.velocity + Vector3.forward * attackForce;
				}
			}
		}

		private void Rotate(Vector3 input)
		{
			float rotation = Mathf.Atan2(input.x, input.z)*Mathf.Rad2Deg;
			transform.Rotate(new Vector3(0, rotation, 0));
		}

		public void OnAnimatorMove()
		{
			if (m_IsGrounded && Time.deltaTime > 0)
			{
				Vector3 v = m_Rigidbody.velocity;

				v = transform.forward * m_ForwardAmount * runSpeed;
				v.y = m_Rigidbody.velocity.y;

				m_Rigidbody.velocity = v;

//				m_Rigidbody.MovePosition(transform.position + Vector3.forward * m_ForwardAmount * runSpeed);
			}
		}


		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				m_Animator.applyRootMotion = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}
		}
	}
}
