//-----------------------------------------------------------------------------
// SmokePlumeParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Particles
{
    /// <summary>
    /// SmokePlumeParticleSystem is a specialization of ParticleSystem which sends up a
    /// plume of smoke. The smoke is blown to the right by the wind.
    /// </summary>
    public class SmokePlumeParticleSystem : ParticleSystem
    {
        public SmokePlumeParticleSystem(int howManyEffects)
            : base(howManyEffects)
        {
        }

        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            TextureFilename = "smoke";

            MinInitialSpeed = 20;
            MaxInitialSpeed = 100;

            // we don't want the particles to accelerate at all, aside from what we
            // do in our overriden InitializeParticle.
            MinAcceleration = 0;
            MaxAcceleration = 0;

            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            MinLifetime = 1.0f;
            MaxLifetime = 3.0f;

            MinScale = .1f;
            MaxScale = .4f;

            MinNumParticles = 2;
            MaxNumParticles = 5;

            // rotate slowly, we want a fairly relaxed effect
            MinRotationSpeed = -MathHelper.PiOver4 / 2.0f;
            MaxRotationSpeed = MathHelper.PiOver4 / 2.0f;

			BlendState = BlendState.AlphaBlend;

            DrawOrder = AlphaBlendDrawOrder;
        }

        /// <summary>
        /// PickRandomDirection is overriden so that we can make the particles always 
        /// move have an initial velocity pointing up.
        /// </summary>
        /// <returns>a random direction which points basically up.</returns>
        protected override Vector2 PickRandomDirection()
        {
            // Point the particles somewhere between 80 and 100 degrees.
            // tweak this to make the smoke have more or less spread.
            float radians = BeebappsGame.RandomBetween(
                MathHelper.ToRadians(80), MathHelper.ToRadians(100));

            Vector2 direction = Vector2.Zero;
            // from the unit circle, cosine is the x coordinate and sine is the
            // y coordinate. We're negating y because on the screen increasing y moves
            // down the monitor.
            direction.X = (float)Math.Cos(radians);
            direction.Y = -(float)Math.Sin(radians);
            return direction;
        }

        /// <summary>
        /// InitializeParticle is overridden to add the appearance of wind.
        /// </summary>
        /// <param name="p">the particle to set up</param>
        /// <param name="where">where the particle should be placed</param>
        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);

            // the base is mostly good, but we want to simulate a little bit of wind
            // heading to the right.
            p.Acceleration.X += BeebappsGame.RandomBetween(10, 50);
        }
    }
}
