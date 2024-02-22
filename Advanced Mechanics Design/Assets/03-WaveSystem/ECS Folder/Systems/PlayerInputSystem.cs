using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;


public partial class PlayerInputSystem : SystemBase
{
	private PlayerActions inputActions;

	protected override void OnCreate()
	{
		inputActions = new PlayerActions();

		RequireForUpdate<PlayerTag>();
	}

	protected override void OnStartRunning()
	{
		inputActions.Enable();
		inputActions.Simple.Move.started += Handle_MoveStarted;
		inputActions.Simple.Move.performed += Handle_MovePerformed;
		inputActions.Simple.Move.canceled += Handle_MoveCanceled;
	}

	protected override void OnStopRunning()
	{
		inputActions.Disable();
	}

	protected override void OnUpdate()
	{
		return;
	}

	private void Handle_MoveStarted(InputAction.CallbackContext context)
	{
		//Find Player Tag Entity that has input move component disabled
		foreach((RefRO<PlayerTag> tag, Entity e) in
			SystemAPI.Query<RefRO<PlayerTag>>().WithDisabled<InputMoveComponent>()
			.WithEntityAccess())
		{
			//Enable the comp
			SystemAPI.SetComponentEnabled<InputMoveComponent>(e, true);
		}
	}

	private void Handle_MovePerformed(InputAction.CallbackContext context)
	{
		//Find Input movement that have player tag, Set the value of the component as the context readvalue
		foreach (RefRW<InputMoveComponent> inputMoveComp in
			SystemAPI.Query<RefRW<InputMoveComponent>>().WithAll<PlayerTag>())
		{
			inputMoveComp.ValueRW.value = context.ReadValue<Vector2>();
		}
	}
	private void Handle_MoveCanceled(InputAction.CallbackContext context)
	{
		// Find Player Tag Entity that has input move component enabled
		foreach(EnabledRefRW<InputMoveComponent> inputMoveComp in
			SystemAPI.Query<EnabledRefRW<InputMoveComponent>>().WithAll<PlayerTag>())
		{
			//disable the comp
			inputMoveComp.ValueRW = false;
		}
			
	}
}
