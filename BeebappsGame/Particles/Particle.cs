//-----------------------------------------------------------------------------
// Particle.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Particles
{
    /// <summary>
    /// particles are the little bits that will make up an effect. each effect will
    /// be comprised of many of these particles. They have basic physical properties,
    /// such as position, velocity, acceleration, and rotation. They'll be drawn as
    /// sprites, all layered on top of one another, and will be very pretty.
    /// </summary>
    public class Particle
    {
        // Position, Velocity, and Acceleration represent exactly what their names
        // indicate. They are public fields rather than properties so that users
        // can directly access their .X and .Y properties.
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;

        // how long this particle will "live"
        public float Lifetime { get; set; }

        // how long it has been since initialize was called
        public float TimeSinceStart { get; set; }

        // the scale of this particle
        public float Scale { get; set; }

        // its rotation, in radians
        public float Rotation { get; set; }

        // how fast does it rotate?
        public float RotationSpeed { get; set; }

        // is this particle still alive? once TimeSinceStart becomes greater than
        // Lifetime, the particle should no longer be drawn or updated.
        public bool Active
        {
            get { return TimeSinceStart < Lifetime; }
        }

        
        // initialize is called by ParticleSystem to set up the particle, and prepares
        // the particle for use.
        public void Initialize(Vector2 position, Vector2 velocity, Vector2 acceleration,
            float lifetime, float scale, float rotationSpeed)
        {
            // set the values to the requested values
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
            Lifetime = lifetime;
            Scale = scale;
            RotationSpeed = rotationSpeed;
            
            // reset TimeSinceStart - we have to do this because particles will be
            // reused.
            TimeSinceStart = 0.0f;

            // set rotation to some random value between 0 and 360 degrees.
            Rotation = BeebappsGame.RandomBetween(0, MathHelper.TwoPi);
        }

        // update is called by the ParticleSystem on every frame. This is where the
        // particle's position and that kind of thing get updated.
        public void Update(float dt)
        {
            Velocity += Acceleration * dt;
            Position += Velocity * dt;

            Rotation += RotationSpeed * dt;

            TimeSinceStart += dt;
        }
    }
}
