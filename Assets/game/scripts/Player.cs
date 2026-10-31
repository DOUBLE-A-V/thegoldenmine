using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float colSize;
    public Vector2 velocity;
    public float moveSpeed;
    public SpriteRenderer pickaxe;
    public Breakable nearestBreakable;
    public Vector3 defaultPickaxeScale;

    public float pickaxeHitTimer = 0;
    public float pickaxeHitDelay;
    public float pickaxeDamage;

    public float speedRatio = 0.2f;
    public float collectingRadiusRatio = 0.2f;
    public float miningRadiusRatio = 0.2f;

    public float speedRatioModifier = 1f;
    public float collectingRadiusRatioModifier = 1f;
    public float miningRadiusRatioModifier = 1f;
    public Item handItem;
    public SpriteRenderer shadow;
    public float walkAnimationTimer = 0.1f;
    public bool walkAnimationSwitch = false;

    public void SetHandItem(Item item)
    {
        pickaxeHitDelay = item.delay;
        pickaxeDamage = item.damage;
        pickaxe.sprite = item.image.sprite;
        M.game.handItemImage.sprite = item.image.sprite;
        handItem = item;
        pickaxeHitTimer = pickaxeHitDelay;
        M.game.UpdateHandItemDurability();
        M.game.handItemDurabilityText.DOFade(1, 0.5f).SetEase(Ease.OutExpo);
        M.game.handItemImage.DOFade(1, 0.5f).SetEase(Ease.OutExpo);
        foreach (ItemModifier mod in handItem.modifiers)
        {
            mod.parent = handItem;
            mod.OnEquip();
        }

        speedRatioModifier = 1;
        collectingRadiusRatioModifier = 1;
        miningRadiusRatioModifier = 1;
        M.inventory.Hide();
    }

    public void Awake()
    {
        defaultPickaxeScale = pickaxe.transform.localScale;
    }

    public Tween pickaxeMoveTween;

    private void KillPickaxeMove()
    {
        if (pickaxeMoveTween != null) pickaxeMoveTween.Kill();
    }

    public void UpdatePlayer()
    {
        walkAnimationTimer -= Time.deltaTime;
        if (walkAnimationTimer <= 0)
        {
            walkAnimationTimer = 0.2f;
            transform.DOKill();
            if (walkAnimationSwitch)transform.DORotate(new Vector3(0, 0, 10), 0.2f).SetEase(Ease.OutExpo);
            else transform.DORotate(new Vector3(0, 0, -10), 0.2f).SetEase(Ease.OutExpo);
            walkAnimationSwitch = !walkAnimationSwitch;
        }
        
        shadow.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        if (Input.GetKey(KeyCode.A)) velocity.x = -moveSpeed * speedRatio * speedRatioModifier;
        else if (Input.GetKey(KeyCode.D)) velocity.x = moveSpeed * speedRatio * speedRatioModifier;
        else velocity.x = 0;

        if (Input.GetKey(KeyCode.S)) velocity.y = -moveSpeed * speedRatio * speedRatioModifier;
        else if (Input.GetKey(KeyCode.W)) velocity.y = moveSpeed * speedRatio * speedRatioModifier;
        else velocity.y = 0;

        if (velocity.x != 0 && velocity.y != 0)
        {
            velocity /= 1.41f;
        }
        if (velocity != Vector2.zero)
        {
            M.game.tutorialText.DOKill();
            M.game.tutorialText.DOFade(0, 1f).SetEase(Ease.Linear);

            walkAnimationTimer -= Time.deltaTime;
            if (walkAnimationTimer <= 0)
            {
                walkAnimationTimer = 0.3f;
                transform.DOKill();
                if (walkAnimationSwitch)transform.DORotate(new Vector3(0, 0, 10), 0.3f).SetEase(Ease.OutExpo);
                else transform.DORotate(new Vector3(0, 0, -10), 0.3f).SetEase(Ease.OutExpo);
                walkAnimationSwitch = !walkAnimationSwitch;
            }
        } else
        {
            transform.DOKill();
            transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.OutExpo);
        }

        transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.8f, 6f), Mathf.Clamp(transform.position.y, -5f, 5f), 0);
        if (velocity.x > 0)
        {
            if (!nearestBreakable)pickaxeMoveTween = pickaxe.transform.DOMove(transform.position + new Vector3(0.8f, 0, 0), 0.5f).SetEase(Ease.OutExpo);
            pickaxe.transform.localScale = defaultPickaxeScale;
        }else if (velocity.x < 0)
        {
            if (!nearestBreakable)pickaxeMoveTween = pickaxe.transform.DOMove(transform.position + new Vector3(-0.8f, 0, 0), 0.5f).SetEase(Ease.OutExpo);
            pickaxe.transform.localScale = new Vector3(-defaultPickaxeScale.x, defaultPickaxeScale.y, 1);
        } else
        {
            if (!nearestBreakable)pickaxeMoveTween = pickaxe.transform.DOMove(new Vector3(pickaxe.transform.position.x, transform.position.y, 0), 0.5f).SetEase(Ease.OutExpo);
        }

        nearestBreakable = null;
        float nearestDistance = 10000;
        foreach (Breakable ore in M.game.breakables)
        {
            float distance = Vector2.Distance(transform.position, ore.transform.position);
            if (distance < ore.colSize + colSize * miningRadiusRatio * miningRadiusRatioModifier + 0.8f && distance < nearestDistance)
            {
                nearestBreakable = ore;
                nearestDistance = distance;
            }
        }

        pickaxeHitTimer -= Time.deltaTime;

        if (nearestBreakable)
        {
            if (pickaxeHitTimer < 0)
            {
                if (nearestBreakable.breakableName == "ore")
                {
                    foreach (ItemModifier mod in handItem.modifiers)
                    {
                        mod.OnHitBreakable(nearestBreakable);
                    }
                    if (nearestBreakable.health.maxHealth - nearestBreakable.health.damage - pickaxeDamage * M.game.itemDamageRatio <= 0)
                    {
                        foreach (ItemModifier mod in handItem.modifiers)
                        {
                            mod.OnMinedOre((OreNode)nearestBreakable);
                        }
                    }
                }

                nearestBreakable.Damage(pickaxeDamage);
                if (handItem)handItem.durability.damage += 1;
                M.game.UpdateHandItemDurability();
            }
            if (nearestBreakable.transform.position.x > transform.position.x)
            {
                if (pickaxeHitTimer < 0)
                {
                    pickaxeHitTimer = pickaxeHitDelay / M.game.itemDelayRatio;
                    pickaxe.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    pickaxe.transform.DOKill();
                    pickaxe.transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
                }
                pickaxe.transform.localScale = defaultPickaxeScale;
            } else
            {
                if (pickaxeHitTimer < 0)
                {
                    pickaxeHitTimer = pickaxeHitDelay / M.game.itemDelayRatio;
                    pickaxe.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    pickaxe.transform.DOKill();
                    pickaxe.transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
                }
                pickaxe.transform.localScale = new Vector3(-defaultPickaxeScale.x, defaultPickaxeScale.y, 1);
            }
            pickaxeMoveTween = pickaxe.transform.DOMove(
                nearestBreakable.transform.position - (nearestBreakable.transform.position - transform.position)
                .normalized * (nearestBreakable.colSize / 1.5f), 0.5f).SetEase(Ease.OutExpo);
        }
    }
}
