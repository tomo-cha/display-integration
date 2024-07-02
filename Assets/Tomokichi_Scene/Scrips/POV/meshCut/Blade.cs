using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME.Extensions;
using System.Linq;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace BLINDED_AM_ME
{
	public class Blade : MonoBehaviour
	{
		public Material CapMaterial;
		public GameObject _target;

		private CancellationTokenSource _previousTaskCancel;
		private bool isRunning = false;

		void Start(){

		}
		void Update(){
			// if (Input.GetKeyDown(KeyCode.Space)){
    		// 	Debug.Log("Spaceキーを押した");
				StartCoroutine("coRoutine");
			// }
		}

		IEnumerator coRoutine()
		{
			if (isRunning){
        		yield break;
			}
    		isRunning = true;

			yield return new WaitForSeconds(0.1f);
    		// 	Debug.Log("Spaceキーを押した");

				if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
				{
					_target.SetActive(false);
					// GameObject original = Instantiate(hit.collider.gameObject, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
					// original.SetActive(false);

					var timeLimit = new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token;

					// this will hold up everything
					Cut(hit.collider.gameObject, timeLimit);

					// original.SetActive(true);
				}
				else
				{
					Debug.LogError("Missed");
					_target.SetActive(true);

        			GameObject[] rightObjects = GameObject.FindGameObjectsWithTag("Right");
					foreach (GameObject right in rightObjects)
					{
						Destroy(right);
					}
				}
			isRunning = false;
		}

		// this will hold up the UI thread
		private void Cut(GameObject target, CancellationToken cancellationToken = default)
		{
			try
			{
				_previousTaskCancel?.Cancel();
				_previousTaskCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
				cancellationToken = _previousTaskCancel.Token;
				cancellationToken.ThrowIfCancellationRequested();

				// get the victims mesh
				var leftSide = Instantiate(target, target.transform.position, target.transform.rotation);
				target.SetActive(false);
				// var leftSide = target;
				var leftMeshFilter = leftSide.GetComponent<MeshFilter>();
				var leftMeshRenderer = leftSide.GetComponent<MeshRenderer>();

				var materials = new List<Material>();
				leftMeshRenderer.GetSharedMaterials(materials);

				// the insides
				var capSubmeshIndex = 0;
				if (materials.Contains(CapMaterial))
					capSubmeshIndex = materials.IndexOf(CapMaterial);
				else
				{
					capSubmeshIndex = materials.Count;
					materials.Add(CapMaterial);
				}

				// set the blade relative to victim
				var blade = new Plane(
					leftSide.transform.InverseTransformDirection(transform.up),
					leftSide.transform.InverseTransformPoint(transform.position));

				var mesh = leftMeshFilter.sharedMesh;
				//var mesh = leftMeshFilter.mesh;

				// Cut
				var pieces = mesh.Cut(blade, capSubmeshIndex, cancellationToken);

				leftSide.name = "LeftSide";
				leftMeshFilter.mesh = pieces.Item1;
				leftMeshRenderer.sharedMaterials = materials.ToArray();
				//leftMeshRenderer.materials = materials.ToArray();

				var rightSide = new GameObject("RightSide");
				rightSide.tag = "Right";
				var rightMeshFilter = rightSide.AddComponent<MeshFilter>();
				var rightMeshRenderer = rightSide.AddComponent<MeshRenderer>();

				rightSide.transform.SetPositionAndRotation(leftSide.transform.position, leftSide.transform.rotation);
				rightSide.transform.localScale = leftSide.transform.localScale;

				rightMeshFilter.mesh = pieces.Item2;
				rightMeshRenderer.sharedMaterials = materials.ToArray();
				//rightMeshRenderer.materials = materials.ToArray();

				// Physics 
				Destroy(leftSide.GetComponent<Collider>());

				// Replace
				var leftCollider = leftSide.AddComponent<MeshCollider>();
				leftCollider.convex = true;
				leftCollider.sharedMesh = pieces.Item1;

				var rightCollider = rightSide.AddComponent<MeshCollider>();
				rightCollider.convex = true;
				rightCollider.sharedMesh = pieces.Item2;

				// rigidbody
				// if (!leftSide.GetComponent<Rigidbody>())
				// 	leftSide.AddComponent<Rigidbody>();

				// if (!rightSide.GetComponent<Rigidbody>())
				// 	rightSide.AddComponent<Rigidbody>();

				Destroy(leftSide);
				// leftSide.SetActive(false);
				// Destroy(rightSide);

			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
		}
		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;

			var top = transform.position + transform.up * 0.5f;
			var bottom = transform.position - transform.up * 0.5f;

			Gizmos.DrawRay(top, transform.forward * 5.0f);
			Gizmos.DrawRay(transform.position, transform.forward * 5.0f);
			Gizmos.DrawRay(bottom, transform.forward * 5.0f);
			Gizmos.DrawLine(top, bottom);
		}
	}
}
