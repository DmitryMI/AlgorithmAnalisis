#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <conio.h>

#define STUPID_VARIANT

#define BODIES_COUNT 6
#define MIN_RADIUS 1E6
#define MAX_RADIUS 1E7
#define MIN_MASS 6E23
#define MAX_MASS 6E24
#define MIN_X 0
#define MAX_X 1e100
#define MIN_Y 0
#define MAX_Y 1e100

#define G 6.67E-11

typedef struct Vector3
{
	double x, y, z;
} vector3;

typedef struct Celestial_body
{
	double mass;
	double radius;
	vector3 position;
	
} celestial_body;



double random_double(double min, double max)
{
	double generated = (double)rand() / (double)RAND_MAX;
	return generated * (max - min) + min;
}

vector3 random_vector3(double x_min, double x_max, double y_min, double y_max, double z_min, double z_max)
{
	vector3 result;
	result.x = random_double(x_min, x_max);
	result.y = random_double(y_min, y_max);
	result.z = random_double(z_min, z_max);
	return result;
}

void generate_bodies(celestial_body *bodies, int count)
{
	for(int i = 0; i < count; i++)
	{
		bodies[i].mass = random_double(MIN_MASS, MAX_MASS);
		bodies[i].radius = random_double(MIN_RADIUS, MAX_RADIUS);
		bodies[i].position = random_vector3(MIN_X, MAX_X, MIN_Y, MAX_Y, 0, 0);
	}
}

vector3 summ_vectors(const vector3 *a, const vector3 *b)
{
	vector3 result = *a;

	result.x += b->x;
	result.y += b->y;
	result.z += b->z;

	return result;
}

vector3 sub_vectors(const vector3 *a, const vector3 *b)
{
	vector3 result = *a;
	result.x -= b->x;
	result.y -= b->y;
	result.z -= b->z;

	return result;
}

void mult_vector(vector3 *a, double k)
{
	a->x *= k;
	a->y *= k;
	a->z *= k;
}

double calculate_force(double distance, double m1, double m2)
{
	return G * m1 * m2 / distance / distance;
}

double get_modulo(const vector3 *a)
{
	double dx = a->x;
	double dy = a->y;
	double dz = a->z;
	return sqrt(dx * dx + dy * dy + dz * dz);
}

vector3 negate(const vector3 *vec)
{
	vector3 result;
	result.x = -vec->x;
	result.y = -vec->y;
	result.z = -vec->z;

	return result;
}

void normalize(vector3 *vec)
{
	double modulo = get_modulo(vec);
	mult_vector(vec, 1 / modulo);
}

double get_distance(const vector3 *a, const vector3 *b)
{
	double dx = a->x - b->x;
	double dy = a->y - b->y;
	double dz = a->z - b->z;
	return sqrt(dx * dx + dy * dy + dz * dz);
}

void calculate_forces(celestial_body *bodies, vector3 *forces, int count)
{
	for (int i = 0; i < count; i++)
	{
		forces[i].x = 0;
		forces[i].y = 0;
		forces[i].z = 0;
	}



	for (int i = 0; i < count; i++)
	{
#ifdef STUPID_VARIANT
		for (int j = 0; j < count; j++)
		{

			if (i == j)
				continue;

			vector3 direction = sub_vectors(&bodies[j].position, &bodies[i].position);
			normalize(&direction);

			double distance = get_distance(&bodies[j].position, &bodies[i].position);
			double force_power = calculate_force(distance, bodies[j].mass, bodies[i].mass);

			vector3 force_part = direction;
			mult_vector(&force_part, force_power);

			forces[i] = summ_vectors(&forces[i], &force_part);
		}
#else
		for (int j = i + 1; j < count; j++)
		{
			vector3 direction = sub_vectors(&bodies[j].position, &bodies[i].position);
			normalize(&direction);

			double distance = get_distance(&bodies[j].position, &bodies[i].position);
			double force_power = calculate_force(distance, bodies[j].mass, bodies[i].mass);

			vector3 force_part_i = direction;
			vector3 force_part_j = negate(&direction);
			mult_vector(&force_part_i, force_power);
			mult_vector(&force_part_j, force_power);

			forces[i] = summ_vectors(&forces[i], &force_part_i);
			forces[j] = summ_vectors(&forces[j], &force_part_j);
		}
#endif		
	}
}

char *vector3_tostr(const vector3 *vec)
{
	char *result = malloc(100);
	sprintf_s(result, 100, "%e, %e, %e", vec->x, vec->y, vec->z);
	return result;
}

void print_results(celestial_body *bodies, vector3 *forces, int count)
{
	for(int i = 0; i < count; i++)
	{
		char *position_str = vector3_tostr(&bodies[i].position);
		char *force_str = vector3_tostr(&forces[i]);
		printf("Position: %s; mass: %3.2f; force: %s\n", position_str, bodies[i].mass, force_str);

		free(position_str);
		free(force_str);
	}
}

int main(void)
{
	celestial_body bodies[BODIES_COUNT];
	vector3 forces[BODIES_COUNT];

	generate_bodies(bodies, BODIES_COUNT);

	calculate_forces(bodies, forces, BODIES_COUNT);

	print_results(bodies, forces, BODIES_COUNT);

	_getch();
}

