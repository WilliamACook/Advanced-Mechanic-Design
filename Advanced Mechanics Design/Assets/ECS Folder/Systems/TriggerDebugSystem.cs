using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Burst;
using Unity.Physics.Systems;
using System.ComponentModel;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct TriggerDebugSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SimulationSingleton>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Dependency = new TriggerDebugJob
		{
			LookupPlayerTag = state.GetComponentLookup<PlayerTag>(),
			deltaTime = SystemAPI.Time.DeltaTime
		}.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
	}

	private partial struct TriggerDebugJob : ITriggerEventsJob
	{
		public float deltaTime;
		public ComponentLookup<PhysicsVelocity> LookupPhysicsVelocity;
		public ComponentLookup<PhysicsMass> LookupPhysicsMass;
		public ComponentLookup<PlayerTag> LookupPlayerTag;
		public ComponentLookup<TriggerTag> LookupTriggerTag;

		public void Execute(TriggerEvent triggerEvent)
		{
			bool isBodyAPlayer = LookupPlayerTag.HasComponent(triggerEvent.EntityA);
			bool isBodyBPlayer = LookupPlayerTag.HasComponent(triggerEvent.EntityB);

			if(!isBodyAPlayer && !isBodyBPlayer) { return; }

			bool isBodyATrigger = LookupTriggerTag.HasComponent(triggerEvent.EntityA);
			bool isBodyBTrigger = LookupTriggerTag.HasComponent(triggerEvent.EntityB);

			if(!isBodyATrigger && !isBodyBTrigger) { return; }

			Entity playerEntity = isBodyAPlayer ? triggerEvent.EntityA : triggerEvent.EntityB;
			deltaTime++;
			UnityEngine.Debug.Log(triggerEvent.EntityA + " " + deltaTime);
		}
	}
}
