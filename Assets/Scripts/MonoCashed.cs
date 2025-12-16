using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoCashed : MonoBehaviour, System.Collections.IEnumerable
    {
        public new Transform transform { get; private set; }
        protected virtual void Awake() { transform = base.transform; PostAwake(); }
        protected virtual void PostAwake() {}

        public Vector3 position { get => transform.position; set => transform.position = value; }
        public Vector3 localPosition { get => transform.localPosition; set => transform.localPosition = value; }
        public Vector3 eulerAngles { get => transform.eulerAngles; set => transform.eulerAngles = value; }
        public Vector3 localEulerAngles { get => transform.localEulerAngles; set => transform.localEulerAngles = value; }
        public Vector3 right { get => transform.right; set => transform.right = value; }
        public Vector3 left { get => -transform.right; set => transform.right = -value; }
        public Vector3 up { get => transform.up; set => transform.up = value; }
        public Vector3 down { get => -transform.up; set => transform.up = -value; }
        public Vector3 forward { get => transform.forward; set => transform.forward = value; }
        public Vector3 back { get => -transform.forward; set => transform.forward = -value; }
        public Quaternion rotation { get => transform.rotation; set => transform.rotation = value; }
        public Quaternion localRotation { get => transform.localRotation; set => transform.localRotation = value; }
        public Vector3 localScale { get => transform.localScale; set => transform.localScale = value; }
        public Transform parent { get => transform.parent; set => transform.parent = value; }
        public bool hasChanged { get => transform.hasChanged; set => transform.hasChanged = value; }
        public int hierarchyCapacity { get => transform.hierarchyCapacity; set => transform.hierarchyCapacity = value; }
        public Matrix4x4 worldToLocalMatrix => transform.worldToLocalMatrix;
        public Matrix4x4 localToWorldMatrix => transform.localToWorldMatrix;
        public Transform root => transform.root;
        public int childCount => transform.childCount;
        public Vector3 lossyScale => transform.lossyScale;
        public int hierarchyCount => transform.hierarchyCount;

        public void SetParent(Transform parent, bool worldPositionStays = true) => transform.SetParent(parent, worldPositionStays);
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation) => transform.SetPositionAndRotation(position, rotation);
        public void SetLocalPositionAndRotation(Vector3 localPosition, Quaternion localRotation) => transform.SetLocalPositionAndRotation(localPosition, localRotation);
        public void GetPositionAndRotation(out Vector3 position, out Quaternion rotation) => transform.GetPositionAndRotation(out position, out rotation);
        public void GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation) => transform.GetLocalPositionAndRotation(out localPosition, out localRotation);
        public void Translate(Vector3 translation, Space relativeTo = Space.Self) => transform.Translate(translation, relativeTo);
        public void Translate(float x, float y, float z, Space relativeTo = Space.Self) => transform.Translate(x, y, z, relativeTo);
        public void Translate(Vector3 translation, Transform relativeTo) => transform.Translate(translation, relativeTo);
        public void Translate(float x, float y, float z, Transform relativeTo) => transform.Translate(x, y, z, relativeTo);
        public void Rotate(Vector3 eulers, Space relativeTo = Space.Self) => transform.Translate(eulers, relativeTo);
        public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo = Space.Self) => transform.Translate(xAngle, yAngle, zAngle, relativeTo);
        public void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self) => transform.Rotate(axis, angle, relativeTo);
        public void RotateAround(Vector3 point, Vector3 axis, float angle) => transform.RotateAround(point, axis, angle);
        public void LookAt(Transform target, Vector3 worldUp) => transform.LookAt(target, worldUp);
        public void LookAt(Transform target) => transform.LookAt(target.position, Vector3.up);
        public void LookAt(Vector3 worldPosition, Vector3 worldUp) => transform.LookAt(worldPosition, worldUp);
        public void LookAt(Vector3 worldPosition) => transform.LookAt(worldPosition, Vector3.up);
        public Vector3 TransformDirection(Vector3 direction) => transform.TransformDirection(direction);
        public Vector3 TransformDirection(float x, float y, float z) => transform.TransformDirection(x, y, z);
        public void TransformDirections(System.ReadOnlySpan<Vector3> directions, System.Span<Vector3> transformedDirections) => transform.TransformDirections(directions, transformedDirections);
        public void TransformDirections(System.Span<Vector3> directions) => transform.TransformDirections(directions);
        public Vector3 InverseTransformDirection(Vector3 direction) => transform.InverseTransformDirection(direction);
        public Vector3 InverseTransformDirection(float x, float y, float z) => transform.InverseTransformDirection(x, y, z);
        public void InverseTransformDirections(System.ReadOnlySpan<Vector3> directions, System.Span<Vector3> transformedDirections) => transform.InverseTransformDirections(directions, transformedDirections);
        public void InverseTransformDirections(System.Span<Vector3> directions) => transform.InverseTransformDirections(directions);
        public Vector3 TransformVector(Vector3 vector) => transform.TransformVector(vector);
        public Vector3 TransformVector(float x, float y, float z) => transform.TransformVector(x, y, z);
        public void TransformVectors(System.ReadOnlySpan<Vector3> vectors, System.Span<Vector3> transformedVectors) => transform.TransformVectors(vectors, transformedVectors);
        public void TransformVectors(System.Span<Vector3> vectors) => transform.TransformVectors(vectors);
        public Vector3 InverseTransformVector(Vector3 vector) => transform.InverseTransformVector(vector);
        public Vector3 InverseTransformVector(float x, float y, float z) => transform.InverseTransformVector(x, y, z);
        public void InverseTransformVectors(System.ReadOnlySpan<Vector3> vectors, System.Span<Vector3> transformedVectors) => transform.InverseTransformVectors(vectors, transformedVectors);
        public void InverseTransformVectors(System.Span<Vector3> vectors) => transform.InverseTransformVectors(vectors);
        public Vector3 TransformPoint(Vector3 position) => transform.TransformPoint(position);
        public Vector3 TransformPoint(float x, float y, float z) => transform.TransformPoint(x, y, z);
        public void TransformPoints(System.ReadOnlySpan<Vector3> positions, System.Span<Vector3> transformedPositions) => transform.TransformPoints(positions, transformedPositions);
        public void TransformPoints(System.Span<Vector3> positions) => transform.TransformPoints(positions);
        public Vector3 InverseTransformPoint(Vector3 position) => transform.InverseTransformPoint(position);
        public Vector3 InverseTransformPoint(float x, float y, float z) => transform.InverseTransformPoint(x, y, z);
        public void InverseTransformPoints(System.ReadOnlySpan<Vector3> positions, System.Span<Vector3> transformedPositions) => transform.InverseTransformPoints(positions, transformedPositions);
        public void InverseTransformPoints(System.Span<Vector3> positions) => transform.InverseTransformPoints(positions);
        public void DetachChildren() => transform.DetachChildren();
        public void SetAsFirstSibling() => transform.SetAsFirstSibling();
        public void SetAsLastSibling() => transform.SetAsLastSibling();
        public void SetSiblingIndex(int index) => transform.SetSiblingIndex(index);
        public int GetSiblingIndex() => transform.GetSiblingIndex();
        public Transform Find(string childName) => transform.Find(childName);
        public bool IsChildOf(Transform parent) => transform.IsChildOf(parent);
        public Transform GetChild(int index) => transform.GetChild(index);

        public System.Collections.IEnumerator GetEnumerator() => transform.GetEnumerator();

        [System.Obsolete("FindChild has been deprecated. Use Find instead (UnityUpgradable) -> Find([mscorlib] System.String)", false)]
        public Transform FindChild(string name) => transform.FindChild(name);

        [System.Obsolete("warning use Transform.Rotate instead.")]
        public void RotateAround(Vector3 axis, float angle) => transform.RotateAround(axis, angle);

        [System.Obsolete("warning use Transform.Rotate instead.")]
        public void RotateAroundLocal(Vector3 axis, float angle) => transform.RotateAroundLocal(axis, angle);

        [System.Obsolete("warning use Transform.childCount instead (UnityUpgradable) -> Transform.childCount", false)]
        public int GetChildCount() => transform.GetChildCount();
    }

public abstract class MonoCashed<T> : MonoCashed
{
    public T Cashed1 { get; private set; }
    protected override void Awake() { Cashed1 = GetComponent<T>(); base.Awake(); }
}

public abstract class MonoCashed<T1, T2> : MonoCashed<T1>
{
    public T2 Cashed2 { get; private set; }
    protected override void Awake() { Cashed2 = GetComponent<T2>(); base.Awake(); }
}

public abstract class MonoCashed<T1, T2, T3> : MonoCashed<T1, T2>
{
    public T3 Cashed3 { get; private set; }
    protected override void Awake() { Cashed3 = GetComponent<T3>(); base.Awake(); }
}

public abstract class MonoCashed<T1, T2, T3, T4> : MonoCashed<T1, T2, T3>
{
    public T4 Cashed4 { get; private set; }
    protected override void Awake() { Cashed4 = GetComponent<T4>(); base.Awake(); }
}

public abstract class MonoCashed<T1, T2, T3, T4, T5> : MonoCashed<T1, T2, T3, T4>
{
    public T5 Cashed5 { get; private set; }
    protected override void Awake() { Cashed5 = GetComponent<T5>(); base.Awake(); }
}

public abstract class MonoCashed<T1, T2, T3, T4, T5, T6> : MonoCashed<T1, T2, T3, T4, T5>
{
    public T6 Cashed6 { get; private set; }
    protected override void Awake() { Cashed6 = GetComponent<T6>(); base.Awake(); }
}

public abstract class MonoCashed<T1, T2, T3, T4, T5, T6, T7> : MonoCashed<T1, T2, T3, T4, T5, T6>
{
    public T7 Cashed7 { get; private set; }
    protected override void Awake() { Cashed7 = GetComponent<T7>(); base.Awake(); }
}

public abstract class MonoCashed<T1, T2, T3, T4, T5, T6, T7, T8> : MonoCashed<T1, T2, T3, T4, T5, T6, T7>
{
    public T8 Cashed8 { get; private set; }
    protected override void Awake() { Cashed8 = GetComponent<T8>(); base.Awake(); }
}
