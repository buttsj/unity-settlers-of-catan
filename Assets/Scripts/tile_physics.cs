using UnityEngine;

public class tile_physics : MonoBehaviour
{
    public bool targeted = false;
    public Vector3 startingX;
    public Quaternion startingQ;

    public bool tile_move = false;
    public bool flip = false;
    Vector3 offset;

    public Vector3 x;                           // position
    public Vector3 v = new Vector3(0, 0, 0);    // velocity
    public Quaternion q = Quaternion.identity;  // quaternion
    public Vector3 w = new Vector3(2, 0, 0);    // angular velocity

    public float m;
    public float mass;                          // mass
    public Matrix4x4 I_body;                    // body inertia

    public float linear_damping;                // for damping
    public float angular_damping;
    public float restitution;                   // for collision

    public Vector3 lastPos = Vector3.zero;
    public Vector3 delta = Vector3.zero;


    // Use this for initialization
    void Start()
    {
        //Initialize coefficients
        w = new Vector3(0, 0, 0);
        x = transform.position;
        startingX = transform.position;
        v = new Vector3(0.1f, 0.1f, 0.1f);
        q = transform.rotation;
        startingQ = transform.rotation;
        linear_damping = 0.999f;
        angular_damping = 0.98f;
        restitution = .5f;     //elastic collision
        m = 1;
        mass = 0;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            mass += m;
            float diag = m * vertices[i].sqrMagnitude;
            I_body[0, 0] += diag;
            I_body[1, 1] += diag;
            I_body[2, 2] += diag;
            I_body[0, 0] -= m * vertices[i][0] * vertices[i][0];
            I_body[0, 1] -= m * vertices[i][0] * vertices[i][1];
            I_body[0, 2] -= m * vertices[i][0] * vertices[i][2];
            I_body[1, 0] -= m * vertices[i][1] * vertices[i][0];
            I_body[1, 1] -= m * vertices[i][1] * vertices[i][1];
            I_body[1, 2] -= m * vertices[i][1] * vertices[i][2];
            I_body[2, 0] -= m * vertices[i][2] * vertices[i][0];
            I_body[2, 1] -= m * vertices[i][2] * vertices[i][1];
            I_body[2, 2] -= m * vertices[i][2] * vertices[i][2];
        }
        I_body[3, 3] = 1;
    }

    Matrix4x4 Get_Cross_Matrix(Vector3 a)
    {
        //Get the cross product matrix of vector a
        Matrix4x4 A = Matrix4x4.zero;
        A[0, 0] = 0;
        A[0, 1] = -a[2];
        A[0, 2] = a[1];
        A[1, 0] = a[2];
        A[1, 1] = 0;
        A[1, 2] = -a[0];
        A[2, 0] = -a[1];
        A[2, 1] = a[0];
        A[2, 2] = 0;
        A[3, 3] = 1;
        return A;
    }

    Matrix4x4 Quaternion_2_Matrix(Quaternion q)
    {
        //Get the rotation matrix R from quaternion q
        Matrix4x4 R = Matrix4x4.zero;
        R[0, 0] = q[3] * q[3] + q[0] * q[0] - q[1] * q[1] - q[2] * q[2];
        R[0, 1] = 2 * (q[0] * q[1] - q[3] * q[2]);
        R[0, 2] = 2 * (q[0] * q[2] + q[3] * q[1]);
        R[1, 0] = 2 * (q[0] * q[1] + q[3] * q[2]);
        R[1, 1] = q[3] * q[3] - q[0] * q[0] + q[1] * q[1] - q[2] * q[2];
        R[1, 2] = 2 * (q[1] * q[2] - q[3] * q[0]);
        R[2, 0] = 2 * (q[0] * q[2] - q[3] * q[1]);
        R[2, 1] = 2 * (q[1] * q[2] + q[3] * q[0]);
        R[2, 2] = q[3] * q[3] - q[0] * q[0] - q[1] * q[1] + q[2] * q[2];
        R[3, 3] = 1;
        return R;
    }

    Quaternion Normalize_Quaternion(Quaternion q)
    {
        Quaternion result;
        float q_length = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        result.x = q.x / q_length;
        result.y = q.y / q_length;
        result.z = q.z / q_length;
        result.w = q.w / q_length;
        return result;
    }

    Quaternion Quaternion_Multiply(Quaternion q1, Quaternion q2)
    {
        Vector3 v1 = new Vector3(q1.x, q1.y, q1.z);
        Vector3 v2 = new Vector3(q2.x, q2.y, q2.z);
        float s1 = q1.w;
        float s2 = q2.w;
        Vector3 v = s1 * v2 + s2 * v1 + Vector3.Cross(v1, v2);
        float s = s1 * s2 - Vector3.Dot(v1, v2);
        Quaternion q = new Quaternion(v.x, v.y, v.z, s);
        return q;
    }


    void Collision_Handler(float dt)
    {
        Matrix4x4 R = Quaternion_2_Matrix(q); // rotational matrix
        Vector3 N = new Vector3(0, 1, 0); // normal vector
        Vector3 r_i_sum = new Vector3(0, 0, 0); // initialize the sum of colliding vertices
        float count = 0f; // number of colliding vertices

        // DETECTION
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 r_i = R * vertices[i]; // pick a vertex
            Vector3 x_i = x + r_i; // position
            Vector3 v_i = v + Vector3.Cross(w, r_i); // velocity

            if (Vector3.Dot(x_i, N) < 0.5f && Vector3.Dot(v_i, N) < 0.5f)
            {
                r_i_sum = r_i_sum + r_i; // add to sum of colliding vertices
                count = count + 1f; // increment to indicate a collision occurred
            }
        }
        // DETECTION END


        // RESPONSE
        if (count != 0)
        {
            Vector3 r_i = r_i_sum / count; // average colliding vertex
            Vector3 v_i = v + Vector3.Cross(w, r_i); // average velocity


            Matrix4x4 R_i = Get_Cross_Matrix(r_i); // rotational matrix
            Matrix4x4 I = R * I_body * R.transpose; // current intertia


            Vector3 top = (-v_i) - (restitution * Vector3.Dot(v_i, N) * N); // multiply 'top' by K.inverse to get j

            Matrix4x4 K = R_i.transpose * I.inverse * R_i; // here is K without (1/mass)*(4x4 identity)
            Matrix4x4 identity = Matrix4x4.identity;
            identity.m00 *= (1 / mass);
            identity.m11 *= (1 / mass); // Multiply the 4x4 identity matrix by (1 / mass)
            identity.m22 *= (1 / mass);
            identity.m33 *= (1 / mass);

            K.m00 += identity.m00;
            K.m01 += identity.m01;
            K.m02 += identity.m02;
            K.m03 += identity.m03;
            K.m10 += identity.m10;
            K.m11 += identity.m11;
            K.m12 += identity.m12;
            K.m13 += identity.m13;
            K.m20 += identity.m20; // Add the two 4x4 Matrices (to calculate final K)
            K.m21 += identity.m21;
            K.m22 += identity.m22;
            K.m23 += identity.m23;
            K.m30 += identity.m30;
            K.m31 += identity.m31;
            K.m32 += identity.m32;
            K.m33 += identity.m33;

            Vector3 j = (K.inverse * top); // calculate j (impulse)

            if (v.y < 1)
            {
                restitution = 0f; // remove oscillation
            }
            else
            {
                restitution = .5f; // keep oscillation
            }

            v += (j / mass); // Apply impulse force from the floor
            w += (Vector3)(I_body.inverse * (Vector3.Cross(r_i, j))); // Apply angular force from floor
        }
        // RESPONSE END
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 fGravity = mass * (new Vector3(0, -9.8f, 0)); // gravity force on bunny
        float dt = 0.02f;
        if (Input.GetKey("l"))
        {
            flip = true;
            restitution = 0.5f;
            v = new Vector3(Random.Range(-50, 50), Random.Range(10, 100), Random.Range(-50, 50));
        }
        if (!tile_move)
        {

            // Part I: Update velocities
            v += (dt * fGravity / mass); // update velocity with gravity (push down)
            v *= linear_damping; // linear damping
            w *= angular_damping; // angular damping

            // Part II: Collision Handler
            Collision_Handler(dt);

            // Part III: Update position & orientation
            //Update linear status
            x += (dt * v);

            transform.position = x;
            transform.rotation = q;
            if (v.x < .01 && v.x > -.01 && v.y < .01 && v.y > -.01 && v.z < .01 && v.z > -.01)
            {
                tile_move = true;
            }
            else
            {
                tile_move = false;
            }
        }
        else
        {
            restitution = .5f;
            transform.position = x;
            transform.rotation = q;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Vector3.Cross(ray.direction, transform.position - ray.origin).magnitude < 2.5f)
            {
                if (Singleton._instance.SetObject == null)
                {
                    tile_move = true;
                    Singleton._instance.SetObject = gameObject;
                    targeted = true;
                }
            }
            offset = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            lastPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Singleton._instance.SetObject == gameObject)
            {
                tile_move = false;
                Singleton._instance.SetObject = null;
                targeted = false;
            }
        }
        if (tile_move && targeted)
        {
            delta = Input.mousePosition - lastPos;
            Vector3 mouse = Input.mousePosition;
            mouse -= offset;
            mouse.z = Camera.main.WorldToScreenPoint(transform.position).z;
            x = Camera.main.ScreenToWorldPoint(mouse);
            v = delta + new Vector3(.1f, .1f, .1f);
            lastPos = Input.mousePosition;
        }
        if (x.y < 0.0f || x.y > 0.0f && flip == false)
        {
            x.y = 0.0f;
            transform.position = x;
        }
    }
}